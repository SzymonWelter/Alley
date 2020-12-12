using System;
using System.Net;
using Grpc.Core;

namespace Alley.Context.Models.Interfaces
{
    public interface IConnectionData<TRequest, TResponse>
        where TRequest : class
        where TResponse : class
    {
        Uri Address { get; }
        Method<TRequest, TResponse> Method { get; }
    }

    public class ConnectionData<TRequest, TResponse> : IConnectionData<TRequest, TResponse> 
        where TRequest : class 
        where TResponse : class
    {
        public Uri Address { get; }
        public Method<TRequest, TResponse> Method { get; }
        public ConnectionData(Uri address, Method<TRequest, TResponse> method)
        {
            Address = address;
            Method = method;
        }
    }
}