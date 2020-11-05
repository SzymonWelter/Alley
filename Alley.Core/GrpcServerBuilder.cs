using System;
using Alley.Core.Factories;
using Alley.Definitions.Mappers.Interfaces;
using Alley.Definitions.Models.Interfaces;
using Grpc.Core;

namespace Alley.Core
{
    public class GrpcServerBuilder
    {
        private readonly IMethodHandlerFactory _methodHandlerFactory;
        private readonly IMethodMapper _methodMapper;
        private readonly Server _server;

        public GrpcServerBuilder(
            IMethodHandlerFactory methodHandlerFactory,
                IMethodMapper methodMapper
        )
        {
            _methodHandlerFactory = methodHandlerFactory;
            _methodMapper = methodMapper;
            _server = new Server();
        }

        public GrpcServerBuilder AddService(IGrpcServiceDefinition serviceDefinition)
        {
            var serverServiceDefinitionBuilder = ServerServiceDefinition.CreateBuilder();

            foreach (var methodDefinition in serviceDefinition.Methods)
            {
                var method = _methodMapper.Map(methodDefinition);
                AddMethod(serverServiceDefinitionBuilder, method);
            }
            _server.Services.Add(serverServiceDefinitionBuilder.Build());
            return this;
        }

        private void AddMethod(ServerServiceDefinition.Builder serverServiceDefinitionBuilder, Method<IAlleyMessageModel, IAlleyMessageModel> method)
        {
            switch(method.Type)
            {
                case MethodType.Unary:
                    var unaryHandler = _methodHandlerFactory.GetUnaryHandler(method);
                    serverServiceDefinitionBuilder.AddMethod(method, unaryHandler);
                    break;
                case MethodType.ClientStreaming:
                    var clientStreamingHandler = _methodHandlerFactory.GetClientStreamingServerHandler(method);
                    serverServiceDefinitionBuilder.AddMethod(method, clientStreamingHandler);
                    break;
                case MethodType.ServerStreaming:
                    var serverStreamingHandler = _methodHandlerFactory.GetServerStreamingServerHandler(method);
                    serverServiceDefinitionBuilder.AddMethod(method, serverStreamingHandler);
                    break;
                case MethodType.DuplexStreaming:
                    var duplexStreamingServerHandler = _methodHandlerFactory.GetDuplexStreamingServerHandler(method);
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

        public GrpcServerBuilder EnableHttp()
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            return this;
        }

        private void ConfigurePorts(ServerPort serverPort)
        {
            _server.Ports.Add(serverPort);
        }
    }
}