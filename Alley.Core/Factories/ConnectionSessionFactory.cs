using System;
using Alley.Context.Models.Interfaces;
using Alley.Core.Session;

namespace Alley.Core.Factories
{
    public class ConnectionSessionFactory<TRequest, TResponse> : IConnectionSessionFactory<TRequest, TResponse> 
        where TRequest : class 
        where TResponse : class
    {
        public IConnectionSession<TRequest, TResponse> Create(IConnectionData<TRequest, TResponse> connectionData)
        {
            throw new NotImplementedException();
        }
    }
}