using System;
using Alley.Definitions.Factories.Interfaces;
using Alley.Definitions.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Alley.Definitions.Factories
{
    public class MicroserviceDefinitionBuilderFactory : IMicroserviceDefinitionBuilderFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public MicroserviceDefinitionBuilderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public IMicroserviceDefinitionBuilder Create()
        {
            return _serviceProvider.GetService<IMicroserviceDefinitionBuilder>();
        }
    }
}