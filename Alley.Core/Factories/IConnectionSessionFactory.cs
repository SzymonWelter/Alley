using Alley.Context.Models.Interfaces;
using Alley.Core.Session;

namespace Alley.Core.Factories
{
    public interface IConnectionSessionFactory<TRequest, TResponse>
        where TRequest : class
        where TResponse : class
    {
        IConnectionSession<TRequest, TResponse> Create(IConnectionData<TRequest, TResponse> connectionData);
    }
}