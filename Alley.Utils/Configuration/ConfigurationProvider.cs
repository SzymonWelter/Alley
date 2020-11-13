using System.IO;
using System.IO.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Alley.Utils.Configuration
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private readonly IConfiguration _configuration;
        public string ProtoPattern => "*.proto";

        public ConfigurationProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDirectoryInfo GetProtosLocalization()
        {
            var path = _configuration["Protos:Localization"];
            var directoryInfo = new DirectoryInfo(path);
            var fileSystem = new FileSystem();
            return new DirectoryInfoWrapper(fileSystem, directoryInfo);
        }

    }
}