using System.Collections;
using System.Collections.Generic;
using Alley.Definitions.Models.Interfaces;
using Grpc.Core;

namespace Alley.Core
{
    public interface IGrpcServerBuilder<TRequest, TResponse> where TRequest : class where TResponse : class
    {
        GrpcServerBuilder<TRequest, TResponse> AddServices(IEnumerable<IGrpcServiceDefinition> serviceDefinitions);
        GrpcServerBuilder<TRequest, TResponse> AddService(IGrpcServiceDefinition serviceDefinition);
        AlleyServer Build();
        IGrpcServerBuilder<TRequest, TResponse> EnableHttp();
        IGrpcServerBuilder<TRequest, TResponse> ConfigurePort(int port);
        void ConfigurePorts(ServerPort serverPort);
    }
}