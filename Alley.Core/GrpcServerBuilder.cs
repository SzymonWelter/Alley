using System;
using System.Net;
using Alley.Core.Factories;
using Alley.Definitions.Mappers.Interfaces;
using Alley.Definitions.Models.Interfaces;
using Grpc.Core;

namespace Alley.Core
{
    public class GrpcServerBuilder<TRequest, TResponse> 
        where TRequest : class 
        where TResponse : class
    {
        private readonly IMethodHandlerFactory<TRequest, TResponse> _methodHandlerFactory;
        private readonly IMethodFactory<TRequest, TResponse> _methodFactory;
        private readonly Server _server;
        private bool _httpEnabled;

        public GrpcServerBuilder(
            IMethodHandlerFactory<TRequest, TResponse> methodHandlerFactory,
                IMethodFactory<TRequest, TResponse> methodFactory
        )
        {
            _methodHandlerFactory = methodHandlerFactory;
            _methodFactory = methodFactory;
            _server = new Server();
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
                    var unaryHandler = _methodHandlerFactory.GetUnaryHandler();
                    serverServiceDefinitionBuilder.AddMethod(method, unaryHandler);
                    break;
                case MethodType.ClientStreaming:
                    var clientStreamingHandler = _methodHandlerFactory.GetClientStreamingServerHandler();
                    serverServiceDefinitionBuilder.AddMethod(method, clientStreamingHandler);
                    break;
                case MethodType.ServerStreaming:
                    var serverStreamingHandler = _methodHandlerFactory.GetServerStreamingServerHandler();
                    serverServiceDefinitionBuilder.AddMethod(method, serverStreamingHandler);
                    break;
                case MethodType.DuplexStreaming:
                    var duplexStreamingServerHandler = _methodHandlerFactory.GetDuplexStreamingServerHandler();
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

        private void ConfigurePorts(ServerPort serverPort)
        {
            _server.Ports.Add(serverPort);
        }
    }
}