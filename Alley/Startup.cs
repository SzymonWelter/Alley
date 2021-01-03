using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Alley.Context;
using Alley.Core;
using Alley.Definitions.Interfaces;
using Alley.Management;
using Alley.Monitoring;
using Alley.Serialization.Models;
using Alley.Utils.Configuration;
using Microsoft.Extensions.Hosting;

namespace Alley
{
    public class Startup
    {
        private readonly IMicroservicesDefinitionsProvider _definitionProvider;
        private readonly IGrpcServerBuilder<IAlleyMessageModel, IAlleyMessageModel> _serverBuilder;
        private readonly IMicroserviceContext _microserviceContext;
        private readonly IManagementServerRunner _managementServerRunner;
        private readonly IMonitoringDirector _monitoringDirector;
        private readonly IConfigurationProvider _configurationProvider;

        public Startup(
            IMicroservicesDefinitionsProvider definitionsProvider,
            IGrpcServerBuilder<IAlleyMessageModel, IAlleyMessageModel> serverBuilder, 
            IMicroserviceContext microserviceContext,
            IManagementServerRunner managementServerRunner,
            IMonitoringDirector monitoringDirector,
            IConfigurationProvider configurationProvider)
        {
            _definitionProvider = definitionsProvider;
            _serverBuilder = serverBuilder;
            _microserviceContext = microserviceContext;
            _managementServerRunner = managementServerRunner;
            _monitoringDirector = monitoringDirector;
            _configurationProvider = configurationProvider;
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

            var grpcServer = _serverBuilder
                .EnableHttp()
                .ConfigurePort(_configurationProvider.GrpcServerPort)
                .Build();

            var grpcServerThread =  new Thread(grpcServer.Run);
            var managementThread = new Thread(_managementServerRunner.Run);
            var monitoringThread = new Thread(_monitoringDirector.Run);
            
            grpcServerThread.Start();
            managementThread.Start();
            monitoringThread.Start();
            grpcServerThread.Join();
        }
    }
}