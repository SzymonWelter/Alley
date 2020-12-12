using Grpc.Core;

namespace Alley.Core.Handling
{
    public class MethodHandlerProvider<TRequest, TResponse> : IMethodHandlerProvider<TRequest, TResponse>
        where TRequest : class
        where TResponse : class
    {
        private readonly ISessionFactory<TRequest, TResponse> _sessionFactory;

        public MethodHandlerProvider(
            ISessionFactory<TRequest, TResponse> sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public UnaryServerMethod<TRequest, TResponse> GetUnaryHandler()
        {
            return async (request, context) =>
            {
                var connection = _sessionFactory.CreateSession(context.Method, MethodType.Unary);
                return await connection.Execute(request, context);
            };
        }

        public ClientStreamingServerMethod<TRequest, TResponse> GetClientStreamingServerHandler()
        {
            return async (requestStream, context) =>
            {
                var connection =  _sessionFactory.CreateSession(context.Method, MethodType.ClientStreaming);
                return await connection.Execute(requestStream, context);
            };        
        }

        public ServerStreamingServerMethod<TRequest, TResponse> GetServerStreamingServerHandler()
        {
            return async (request, responseStream, context) =>
            {
                var connection = _sessionFactory.CreateSession(context.Method, MethodType.ServerStreaming);
                await connection.Execute(request, responseStream, context);
            };
        }

        public DuplexStreamingServerMethod<TRequest, TResponse> GetDuplexStreamingServerHandler()
        {
            return async (requestStream, responseStream, context) =>
            {
                var connection = _sessionFactory.CreateSession(context.Method, MethodType.DuplexStreaming);
                await connection.Execute(requestStream, responseStream, context);
            };        
        }
    }
}
