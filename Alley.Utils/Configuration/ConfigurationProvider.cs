using System.IO;
using System.IO.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Alley.Utils.Configuration
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private readonly IConfiguration _configuration;
        private const string ProtosPath = "Protos:Localization";
        private const string ProtocolPath = "Protocol";

        public string ProtoPattern => "*.proto";
        public string Protocol { get; }

        public ConfigurationProvider(IConfiguration configuration)
        {
            _configuration = configuration;
            Protocol = ParseProtocol();
        }

        private string ParseProtocol()
        {
            var protocolText = _configuration[ProtocolPath];
            return protocolText == "https" ? "https" : "http";
        }

        public IDirectoryInfo GetProtosPath()
        {
            var path = _configuration[ProtosPath];
            var directoryInfo = new DirectoryInfo(path);
            var fileSystem = new FileSystem();
            return new DirectoryInfoWrapper(fileSystem, directoryInfo);
        }
    }
}