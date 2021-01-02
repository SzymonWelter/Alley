using System;
using System.Threading.Tasks;
using Alley.Context;
using Alley.Context.Metrics;
using Alley.Utils;
using Alley.Utils.Configuration;
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
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IAlleyLogger _logger;
        private readonly string _target;

        public ConnectionSession(ChannelBase channel,
            Method<TRequest, TResponse> method,
            IMetricRepository metricRepository,
            IConfigurationProvider configurationProvider, 
            IAlleyLogger logger)
        {
            _channel = channel;
            _method = method;
            _metricRepository = metricRepository;
            _configurationProvider = configurationProvider;
            _logger = logger;
            _target = FormatTarget(_channel.Target);
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
                SessionHelper.HandleIfError(result, _logger);

                return response;
            }
            finally
            {
                FinalizeConnection();
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

                SessionHelper.HandleIfError(result, _logger);
                
                using var streamingCall = result.Value;
                var rewriteResult = await RewriteStream(requestStream, streamingCall.RequestStream);
                await streamingCall.RequestStream.CompleteAsync();
                var response = await streamingCall.ResponseAsync;
                SessionHelper.HandleIfError(rewriteResult, _logger);
                return response;
            }
            finally
            {
                FinalizeConnection();
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
                SessionHelper.HandleIfError(result, _logger);
                
                using var serverStreamingCall = result.Value;
                var rewriteResult = await RewriteStream(serverStreamingCall.ResponseStream, responseStream);
                SessionHelper.HandleIfError(rewriteResult, _logger);
            }
            finally
            {
                FinalizeConnection();
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
                SessionHelper.HandleIfError(result, _logger);

                using var streamSource = result.Value;
                var requestRewriteTask = RewriteStream(requestStream, streamSource.RequestStream);
                var responseRewriteTask = RewriteStream(streamSource.ResponseStream, responseStream);

                await Task.WhenAny(requestRewriteTask, responseRewriteTask);
                await streamSource.RequestStream.CompleteAsync();
                SessionHelper.HandleIfError(await requestRewriteTask, _logger);
                SessionHelper.HandleIfError(await responseRewriteTask, _logger);
            }
            finally
            {
                FinalizeConnection();
            }
        }

        private Result<T> ExecuteInternal<T>(Func<CallInvoker ,T> call)
        {
            try
            {
                _logger.Information(Messages.ConnectionStartedWith(_target));
                var callInvoker = _channel.CreateCallInvoker();
                var result = call(callInvoker);
                return Result<T>.Success(result);
            }
            catch (Exception e)
            {
                return Result<T>.Failure(e.Message);
            }
        }
        private void FinalizeConnection()
        {
            DecreaseActiveConnectionCount();
            _logger.Information(Messages.ConnectionEndedWith(_target));
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
        
        private string FormatTarget(string channelTarget)
        {
            var protocol = _configurationProvider.Protocol;
            
            return $"{protocol}://{channelTarget}";
        }
    }
}
