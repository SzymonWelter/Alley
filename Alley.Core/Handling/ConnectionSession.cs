﻿using System;
using System.Threading.Tasks;
using Alley.Context;
using Alley.Context.Metrics;
using Alley.Utils.Helpers;
using Alley.Utils.Models;
using Grpc.Core;

namespace Alley.Core.Handling
{
    public class ConnectionSession<TRequest, TResponse> : IConnectionSession<TRequest, TResponse>
        where TRequest : class 
        where TResponse : class
    {
        private readonly IMetricRepository _metricRepository;
        private readonly Method<TRequest, TResponse> _method;
        private readonly ChannelBase _channel;
        private readonly string _target;

        public ConnectionSession(
            ChannelBase channel, 
            Method<TRequest, TResponse> method, 
            IMetricRepository metricRepository)
        {
            _channel = channel;
            _method = method;
            _metricRepository = metricRepository;
            _target = SessionHelper.FormatTarget(_channel.Target);
        }

        public async Task<TResponse> Execute(TRequest request, ServerCallContext context)
        {
            IncreaseActiveConnectionCount();
            try
            {
                var result = ExecuteInternal(ci => ci.AsyncUnaryCall(_method,
                    _target,
                    CallOptionsHelper.Rewrite(context),
                    request));
                var response = await result.Value;
                SessionHelper.HandleIfError(result);

                return response;
            }
            finally
            {
                DecreaseActiveConnectionCount();
            }
        }


        public async Task<TResponse> Execute(IAsyncStreamReader<TRequest> requestStream, ServerCallContext context)
        {
            IncreaseActiveConnectionCount();
            try
            {
                var result = ExecuteInternal(ci => ci.AsyncClientStreamingCall(
                    _method,
                    _target,
                    CallOptionsHelper.Rewrite(context)));

                SessionHelper.HandleIfError(result);
                
                using var streamingCall = result.Value;
                var rewriteResult = await RewriteStream(requestStream, streamingCall.RequestStream);
                await streamingCall.RequestStream.CompleteAsync();
                var response = await streamingCall.ResponseAsync;
                SessionHelper.HandleIfError(rewriteResult);
                return response;
            }
            finally
            {
                DecreaseActiveConnectionCount();
            }
        }


        public async Task Execute(TRequest request, IServerStreamWriter<TResponse> responseStream, ServerCallContext context)
        {
            IncreaseActiveConnectionCount();
            try
            {
                var result = ExecuteInternal(ci => ci.AsyncServerStreamingCall(
                    _method,
                    _target,
                    CallOptionsHelper.Rewrite(context),
                    request));
                SessionHelper.HandleIfError(result);
                
                using var serverStreamingCall = result.Value;
                var rewriteResult = await RewriteStream(serverStreamingCall.ResponseStream, responseStream);
                SessionHelper.HandleIfError(rewriteResult);
            }
            finally
            {
                DecreaseActiveConnectionCount();
            }
        }

        public async Task Execute(IAsyncStreamReader<TRequest> requestStream, IServerStreamWriter<TResponse> responseStream, ServerCallContext context)
        {
            IncreaseActiveConnectionCount();
            try
            {
                var result = ExecuteInternal(ci => ci.AsyncDuplexStreamingCall(
                    _method,
                    _target,
                    CallOptionsHelper.Rewrite(context)));
                SessionHelper.HandleIfError(result);

                using var streamSource = result.Value;
                var requestRewriteTask = RewriteStream(requestStream, streamSource.RequestStream);
                var responseRewriteTask = RewriteStream(streamSource.ResponseStream, responseStream);
                await Task.WhenAll(requestRewriteTask, responseRewriteTask);
                SessionHelper.HandleIfError(await requestRewriteTask);
                SessionHelper.HandleIfError(await responseRewriteTask);
            }
            finally
            {
                DecreaseActiveConnectionCount();
            }
        }

        private Result<T> ExecuteInternal<T>(Func<CallInvoker ,T> call)
        {
            try
            {
                var callInvoker = _channel.CreateCallInvoker();
                var result = call(callInvoker);
                return Result<T>.Success(result);
            }
            catch (Exception e)
            {
                return Result<T>.Failure(e.Message);
            }
        }
        
        private void IncreaseActiveConnectionCount()
        {
            UpdateActiveConnectionCount(1);

        }
        
        private void DecreaseActiveConnectionCount()
        {
            UpdateActiveConnectionCount(-1);
        }

        private void UpdateActiveConnectionCount(int change)
        {
            _metricRepository.UpdateMetric(
                new Uri(_target),
                MetricType.ActiveConnection, 
                m => new ActiveConnectionMetric(m.Value + change));
        }
        
        private static async Task<IResult> RewriteStream<T>(IAsyncStreamReader<T> source, IAsyncStreamWriter<T> destination) where T : class
        {
            try
            {
                while (await source.MoveNext())
                {
                    await destination.WriteAsync(source.Current);
                }
            }
            catch (Exception e)
            {
                return Result.Failure(e.Message);
            }
            return Result.Success();
        }
    }
}