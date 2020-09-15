using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Alley.Core.Factories;
using Alley.Core.Models;
using Alley.Core.Providers;
using Alley.Core.Services;
using Alley.Core.Utilities;
using Google.Protobuf.Reflection;
using Grpc.Core;
using Serilog;

namespace Alley.Core
{
    public class GrpcServerBuilder
    {
        private const string SearchPattern = "*.proto";
        private readonly FileDescriptorSet _fileDescriptorSet = new FileDescriptorSet();
        private readonly IConfigurationService _configuration;
        private readonly MethodFactory _methodFactory;
        private readonly AlleyMethodHandlerProvider _alleyMethodHandlerProvider;

        public GrpcServerBuilder(IConfigurationService configuration)
        {
            _configuration = configuration;
            _methodFactory = new MethodFactory();
            _alleyMethodHandlerProvider = new AlleyMethodHandlerProvider();
        }

        public Server Build()
        {
            var server = new Server();
            var serviceBuilder = ServerServiceDefinition.CreateBuilder();

            var methodModels = GetMethodModels();
            
            foreach ( var methodModel in methodModels)
            {
                AddMethodToServiceBuilder(serviceBuilder, methodModel);
            }
            
            server.Ports.Add("localhost", 5000, ServerCredentials.Insecure);
            server.Services.Add(serviceBuilder.Build());

            return server;
        }

        private IEnumerable<AlleyMethodModel> GetMethodModels()
        {
            var methodModelFactory = new MethodModelFactory();
            return
                from fileDescriptor in _fileDescriptorSet.Files
                from service in fileDescriptor.Services
                from method in service.Methods
                select 
                    methodModelFactory.Create(
                        fileDescriptor,
                        service,
                        method
                    );
        }

        public GrpcServerBuilder ConfigureFromProtos(DirectoryInfo protosLocalization)
        {
            var filesInfo = protosLocalization.EnumerateFiles(SearchPattern);
            foreach (var fileInfo in filesInfo)
            {
                using var file = File.OpenRead(fileInfo.FullName);
                _fileDescriptorSet.Add(fileInfo.Name,true, new StreamReader(file));
            }
            _fileDescriptorSet.Process();
            return this;
        }

        public GrpcServerBuilder EnableHttp()
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            return this;
        }

        private void AddMethodToServiceBuilder(ServerServiceDefinition.Builder serviceBuilder, AlleyMethodModel methodModel)
        {
            var methodCreationResult = _methodFactory.Create(methodModel);
            if (methodCreationResult.IsFailure)
            {
                return;
            }

            var addingResult = AddMethod(serviceBuilder, methodCreationResult.Value);
            if (addingResult.IsFailure)
            {
                 Log.Error(LogMessageFactory.Create(addingResult.ErrorMessage));
            }
        }

        private Result AddMethod(ServerServiceDefinition.Builder serviceBuilder, Method<IAlleyMessageModel, IAlleyMessageModel> methodModel)
        {
            
            
            switch (methodModel.Type)
            {
                case MethodType.Unary:
                    serviceBuilder.AddMethod(
                        methodModel,
                        _alleyMethodHandlerProvider.GetUnaryHandler(methodModel));
                    break;
                case MethodType.ClientStreaming:
                    serviceBuilder.AddMethod(
                        methodModel,
                        _alleyMethodHandlerProvider.GetClientStreamingHandler(methodModel));
                    break;
                case MethodType.ServerStreaming:
                    serviceBuilder.AddMethod(
                        methodModel,
                        _alleyMethodHandlerProvider.GetServerStreamingHandler(methodModel));
                    break;
                case MethodType.DuplexStreaming:
                    serviceBuilder.AddMethod(
                        methodModel,
                        _alleyMethodHandlerProvider.GetDuplexStreamingHandler(methodModel));
                    break;
                default:
                    return Result.Failure(new ArgumentOutOfRangeException().ToString());
            }
            return Result.Success();
        }
    }
}