using Alley.Context.Models.Interfaces;
using Grpc.Core;

namespace Alley.Core.Handling
{
    public interface ISessionFactory<TRequest, TResponse>
        where TRequest : class
        where TResponse : class
    {
        IConnectionSession<TRequest, TResponse> CreateSession(string methodFullName, MethodType methodType);
    }
}