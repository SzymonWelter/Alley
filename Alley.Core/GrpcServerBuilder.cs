using System;
using System.Collections.Generic;
using System.Net;
using Alley.Core.Handling;
using Alley.Definitions.Mappers.Interfaces;
using Alley.Definitions.Models.Interfaces;
using Grpc.Core;

namespace Alley.Core
{
    public class GrpcServerBuilder<TRequest, TResponse> : IGrpcServerBuilder<TRequest, TResponse> 
        where TRequest : class 
        where TResponse : class
    {
        private readonly IMethodHandlerProvider<TRequest, TResponse> _methodHandlerProvider;
        private readonly IMethodFactory<TRequest, TResponse> _methodFactory;
        private readonly Server _server;
        private bool _httpEnabled;

        public GrpcServerBuilder(
            IMethodHandlerProvider<TRequest, TResponse> methodHandlerProvider,
            IMethodFactory<TRequest, TResponse> methodFactory
        )
        {
            _methodHandlerProvider = methodHandlerProvider;
            _methodFactory = methodFactory;
            _server = new Server();
        }
        
        public GrpcServerBuilder<TRequest, TResponse> AddServices(IEnumerable<IGrpcServiceDefinition> serviceDefinitions)
        {
            foreach (var serviceDefinition in serviceDefinitions)
            {
                this.AddService(serviceDefinition);
            }

            return this;
        }

        public GrpcServerBuilder<TRequest, TResponse> AddService(IGrpcServiceDefinition serviceDefinition)
        {
            var serverServiceDefinitionBuilder = ServerServiceDefinition.CreateBuilder();

            foreach (var methodDefinition in serviceDefinition.Methods)
            {
                var method = _methodFactory.Create(methodDefinition);
                AddMethod(serverServiceDefinitionBuilder, method);
            }
            _server.Services.Add(serverServiceDefinitionBuilder.Build());
            return this;
        }

        private void AddMethod(ServerServiceDefinition.Builder serverServiceDefinitionBuilder, Method<TRequest, TResponse> method)
        {
            switch(method.Type)
            {
                case MethodType.Unary:
                    var unaryHandler = _methodHandlerProvider.GetUnaryHandler();
                    serverServiceDefinitionBuilder.AddMethod(method, unaryHandler);
                    break;
                case MethodType.ClientStreaming:
                    var clientStreamingHandler = _methodHandlerProvider.GetClientStreamingServerHandler();
                    serverServiceDefinitionBuilder.AddMethod(method, clientStreamingHandler);
                    break;
                case MethodType.ServerStreaming:
                    var serverStreamingHandler = _methodHandlerProvider.GetServerStreamingServerHandler();
                    serverServiceDefinitionBuilder.AddMethod(method, serverStreamingHandler);
                    break;
                case MethodType.DuplexStreaming:
                    var duplexStreamingServerHandler = _methodHandlerProvider.GetDuplexStreamingServerHandler();
                    serverServiceDefinitionBuilder.AddMethod(method, duplexStreamingServerHandler);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public AlleyServer Build()
        {
            return new AlleyServer(_server);
        }

        public GrpcServerBuilder<TRequest, TResponse> EnableHttp()
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            _httpEnabled = true;
            return this;
        }

        public GrpcServerBuilder<TRequest, TResponse> ConfigurePort(int port)
        {
            var serverPort = new ServerPort(
                IPAddress.Any.ToString(),
                port,
                _httpEnabled ? ServerCredentials.Insecure : null);
            _server.Ports.Add(serverPort);
            return this;
        }

        public void ConfigurePorts(ServerPort serverPort)
        {
            _server.Ports.Add(serverPort);
        }
    }
}