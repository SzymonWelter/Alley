using Grpc.Core;
using Alley.Definitions.Models.Interfaces;

namespace Alley.Definitions.Mappers.Interfaces
{
    public interface IMethodFactory<TRequest, TResponse>
    {
        public Method<TRequest, TResponse> Create(IGrpcMethodDefinition methodDefinition);
        Method<TRequest, TResponse> Create(string methodFullName, MethodType methodType);
    }
}