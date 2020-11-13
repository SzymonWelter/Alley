using Grpc.Core;

namespace Alley.Core.Factories
{
    public interface IMethodHandlerFactory<TRequest, TResponse> 
        where TRequest : class 
        where TResponse : class
    {
        UnaryServerMethod<TRequest, TResponse> GetUnaryHandler(); 
        ClientStreamingServerMethod<TRequest, TResponse> GetClientStreamingServerHandler();
        ServerStreamingServerMethod<TRequest, TResponse> GetServerStreamingServerHandler();
        DuplexStreamingServerMethod<TRequest, TResponse> GetDuplexStreamingServerHandler();
    }
}