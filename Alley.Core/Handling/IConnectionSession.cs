﻿using System;
using System.Threading.Tasks;
using Grpc.Core;

namespace Alley.Core.Handling
{
    public interface IConnectionSession<in TRequest, TResponse>
        where TRequest : class
        where TResponse : class
    {
        Task<TResponse> Execute(TRequest request, ServerCallContext context);
        Task<TResponse> Execute(IAsyncStreamReader<TRequest> requestStream, ServerCallContext context);
        Task Execute(TRequest requestStream, IServerStreamWriter<TResponse> responseStream, ServerCallContext context);
        Task Execute(IAsyncStreamReader<TRequest> requestStream, IServerStreamWriter<TResponse> responseStream, ServerCallContext context);
    }
}