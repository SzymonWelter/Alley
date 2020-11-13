using Alley.Context.Models.Interfaces;
using Alley.Definitions.Mappers.Interfaces;
using Grpc.Core;

namespace Alley.Context.Providers
{
    public class ConnectionDataProvider<TRequest, TResponse> : IConnectionDataProvider<TRequest, TResponse> 
        where TRequest : class 
        where TResponse : class
    {
        private readonly ILoadBalancingExecutor _loadBalancingExecutor;
        private readonly IMethodFactory<TRequest, TResponse> _methodFactory;

        public ConnectionDataProvider(ILoadBalancingExecutor loadBalancingExecutor,
            IMethodFactory<TRequest, TResponse> methodFactory)
        {
            _loadBalancingExecutor = loadBalancingExecutor;
            _methodFactory = methodFactory;
        }
        public IConnectionData<TRequest, TResponse> GetConnectionData(string methodFullName, MethodType methodType)
        {
            var method = _methodFactory.Create(methodFullName, methodType);
            var targetIpResult = _loadBalancingExecutor.GetTarget(method.ServiceName);
            if (targetIpResult.IsFailure)
            {
                throw new RpcException(Status.DefaultCancelled, targetIpResult.Message);
            }

            return new ConnectionData<TRequest, TResponse>(targetIpResult.Value, method);
        }
    }
}