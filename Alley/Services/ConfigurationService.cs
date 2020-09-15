using Alley.Core.Services;
using Microsoft.Extensions.Configuration;

namespace Alley.Configuration
{
    internal class ConfigurationService : IConfigurationService
    {
        private IConfiguration _configuration;

        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}