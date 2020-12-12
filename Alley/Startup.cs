using System;
using System.Linq;
using System.Threading.Tasks;
using Alley.Context;
using Alley.Core;
using Alley.Definitions.Interfaces;
using Alley.Serialization.Models;

namespace Alley
{
    public class Startup
    {
        private readonly IMicroservicesDefinitionsProvider _definitionProvider;
        private readonly IGrpcServerBuilder<IAlleyMessageModel, IAlleyMessageModel> _serverBuilder;
        private readonly IMicroserviceContext _microserviceContext;

        public Startup(
            IMicroservicesDefinitionsProvider definitionsProvider,
            IGrpcServerBuilder<IAlleyMessageModel, IAlleyMessageModel> serverBuilder, 
            IMicroserviceContext microserviceContext)
        {
            _definitionProvider = definitionsProvider;
            _serverBuilder = serverBuilder;
            _microserviceContext = microserviceContext;
        }
        public async Task Run()
        {
            var microservicesDefinitions = _definitionProvider.GetMicroservicesDefinitions();
            foreach (var microserviceDefinition in microservicesDefinitions)
            {
                _serverBuilder.AddServices(microserviceDefinition.Services);
                _microserviceContext.RegisterMicroservice(microserviceDefinition.Name,
                    microserviceDefinition.Services.Select(s => s.Name));
            }

            _microserviceContext.RegisterInstance("Counter", new Uri("http://localhost:5001"));

            var server = _serverBuilder
                .EnableHttp()
                .ConfigurePort(5000)
                .Build();
            await server.Run();
        }
    }
}