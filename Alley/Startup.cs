using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Alley.Context;
using Alley.Core;
using Alley.Definitions.Interfaces;
using Alley.Management;
using Alley.Serialization.Models;
using Microsoft.Extensions.Hosting;

namespace Alley
{
    public class Startup
    {
        private readonly IMicroservicesDefinitionsProvider _definitionProvider;
        private readonly IGrpcServerBuilder<IAlleyMessageModel, IAlleyMessageModel> _serverBuilder;
        private readonly IMicroserviceContext _microserviceContext;
        private readonly IManagementServerRunner _managementServerRunner;

        public Startup(
            IMicroservicesDefinitionsProvider definitionsProvider,
            IGrpcServerBuilder<IAlleyMessageModel, IAlleyMessageModel> serverBuilder, 
            IMicroserviceContext microserviceContext,
            IManagementServerRunner managementServerRunner)
        {
            _definitionProvider = definitionsProvider;
            _serverBuilder = serverBuilder;
            _microserviceContext = microserviceContext;
            _managementServerRunner = managementServerRunner;
        }
        public void Run()
        {
            var microservicesDefinitions = _definitionProvider.GetMicroservicesDefinitions();
            foreach (var microserviceDefinition in microservicesDefinitions)
            {
                _serverBuilder.AddServices(microserviceDefinition.Services);
                _microserviceContext.RegisterMicroservice(microserviceDefinition.Name,
                    microserviceDefinition.Services.Select(s => s.Name));
            }

            _microserviceContext.RegisterInstance("Counter", new Uri("http://localhost:5001"));
            _microserviceContext.RegisterInstance("Counter", new Uri("http://localhost:5002"));
            _microserviceContext.RegisterInstance("Counter", new Uri("http://localhost:5003"));
            

            var grpcServer = _serverBuilder
                .EnableHttp()
                .ConfigurePort(5000)
                .Build();

            var grpcServerThread =  new Thread(grpcServer.Run);
            var managementThread = new Thread(_managementServerRunner.Run);
            
            grpcServerThread.Start();
            managementThread.Start();
            grpcServerThread.Join();
        }
    }
}