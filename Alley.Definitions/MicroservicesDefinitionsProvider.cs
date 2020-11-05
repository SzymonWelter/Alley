using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using Alley.Definitions.Interfaces;
using Alley.Definitions.Models.Interfaces;
using Alley.Utils.Configuration;

namespace Alley.Definitions
{
    internal class MicroservicesDefinitionsProvider : IMicroservicesDefinitionsProvider
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IMicroserviceDefinitionBuilder _definitionBuilder;

        public MicroservicesDefinitionsProvider(
            IMicroserviceDefinitionBuilder definitionBuilder,
            IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
            _definitionBuilder = definitionBuilder;
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
            foreach (var file in files)
            {
                _definitionBuilder.AddProto(file);
            }
            return _definitionBuilder.Build(localization.Name);
        }
    }
}