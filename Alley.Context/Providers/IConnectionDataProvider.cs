using Alley.Context.Models.Interfaces;
using Grpc.Core;

namespace Alley.Context.Providers
{
    public interface IConnectionDataProvider<TRequest, TResponse>
        where TRequest : class
        where TResponse : class
    {
        IConnectionData<TRequest, TResponse> GetConnectionData(string methodFullName, MethodType methodType);
    }
}