using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using Alley.Definitions.Factories.Interfaces;
using Alley.Definitions.Interfaces;
using Alley.Definitions.Models.Interfaces;
using Alley.Utils.Configuration;

namespace Alley.Definitions
{
    public class MicroservicesDefinitionsProvider : IMicroservicesDefinitionsProvider
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IMicroserviceDefinitionBuilderFactory _definitionBuilderFactory;

        public MicroservicesDefinitionsProvider(
            IMicroserviceDefinitionBuilderFactory definitionBuilderFactory,
            IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
            _definitionBuilderFactory = definitionBuilderFactory;
        }
        
        public IEnumerable<IMicroserviceDefinition> GetMicroservicesDefinitions()
        {
            var rootProtosDirectory = _configurationProvider.GetProtosLocalization();

            return rootProtosDirectory
                .GetDirectories()
                .Select(GetMicroserviceDefinition);
        }

        private IMicroserviceDefinition GetMicroserviceDefinition(IDirectoryInfo localization)
        {
            var files = localization.GetFiles(_configurationProvider.ProtoPattern);
            var definitionBuilder = _definitionBuilderFactory.Create();
            foreach (var file in files)
            {
                definitionBuilder.AddProto(file);
            }
            return definitionBuilder.Build(localization.Name);
        }
    }
}