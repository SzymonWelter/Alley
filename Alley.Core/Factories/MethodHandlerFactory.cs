using Alley.Context.Providers;
using Grpc.Core;

namespace Alley.Core.Factories
{
    public class MethodHandlerFactory<TRequest, TResponse> : IMethodHandlerFactory<TRequest, TResponse>
        where TRequest : class
        where TResponse : class
    {
        private readonly IConnectionDataProvider<TRequest, TResponse> _connectionDataProvider;
        private readonly IConnectionSessionFactory<TRequest, TResponse> _connectionSessionFactory;

        public MethodHandlerFactory(
            IConnectionDataProvider<TRequest, TResponse> connectionDataProvider, 
            IConnectionSessionFactory<TRequest, TResponse> connectionSessionFactory)
        {
            _connectionDataProvider = connectionDataProvider;
            _connectionSessionFactory = connectionSessionFactory;
        }

        public UnaryServerMethod<TRequest, TResponse> GetUnaryHandler()
        {
            return async (request, context) =>
            {
                var connectionData = _connectionDataProvider.GetConnectionData(context.Method, MethodType.Unary);
                using var connection = _connectionSessionFactory.Create(connectionData);
                return await connection.Execute(request, context);
            };
        }

        public ClientStreamingServerMethod<TRequest, TResponse> GetClientStreamingServerHandler()
        {
            return async (requestStream, context) =>
            {
                var connectionData = _connectionDataProvider.GetConnectionData(context.Method, MethodType.ClientStreaming);
                using var connection = _connectionSessionFactory.Create(connectionData);
                return await connection.Execute(requestStream, context);
            };        
        }

        public ServerStreamingServerMethod<TRequest, TResponse> GetServerStreamingServerHandler()
        {
            return async (request, responseStream, context) =>
            {
                var connectionData = _connectionDataProvider.GetConnectionData(context.Method, MethodType.ServerStreaming);
                using var connection = _connectionSessionFactory.Create(connectionData);
                await connection.Execute(request, responseStream, context);
            };        
        }

        public DuplexStreamingServerMethod<TRequest, TResponse> GetDuplexStreamingServerHandler()
        {
            return async (requestStream, responseStream, context) =>
            {
                var connectionData = _connectionDataProvider.GetConnectionData(context.Method, MethodType.DuplexStreaming);
                using var connection = _connectionSessionFactory.Create(connectionData);
                await connection.Execute(requestStream, responseStream, context);
            };        
        }
    }
}
