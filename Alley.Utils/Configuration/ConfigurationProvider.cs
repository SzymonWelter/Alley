using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Alley.Utils.Configuration
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private readonly IConfiguration _configuration;
        private readonly IDictionary<string, int> _ports;
        private const string CpuUsageSubPath = "CpuUsage";
        private const string HealthCheckSubPath = "HealthCheck";
        private const string MetricQueryPath = "Metrics:{0}:{1}";
        private const string ProtosPath = "Protos:Localization";
        private const string ProtocolPath = "Protocol";
        private const int DefaultTimeout = 10000;
        private const int DefaultGrpcServerPort = 80;

        public string ProtoPattern => "*.proto";
        public string Protocol { get; }
        public string CpuUsageQuery { get; }
        public string HealthCheckQuery { get; }
        public int HealthCheckTimeout { get; }
        public int GrpcServerPort { get; }
        public int MetricsTimeout { get; }
        public int DefaultServicesPort { get; }

        public ConfigurationProvider(IConfiguration configuration)
        {
            _configuration = configuration;
            Protocol = ParseProtocol();
            CpuUsageQuery = ParseMetricQuery(CpuUsageSubPath);
            HealthCheckQuery = ParseMetricQuery(HealthCheckSubPath);
            HealthCheckTimeout = ParseMetricTimeout();
            MetricsTimeout = ParseMetricTimeout();
            GrpcServerPort = ParseGrpcServerPort();
            DefaultServicesPort = ParseDefaultServicesPort();
            _ports = ParsePorts();
        }


        private int ParseDefaultServicesPort()
        {
            var value = _configuration["Services:default:Port"];
            return int.TryParse(value, out var port) ? port : 80;
        }

        private IDictionary<string, int> ParsePorts()
        {
            var dictionary = new Dictionary<string, int>();
            var services = _configuration.GetSection("Services").GetChildren();
            foreach (var service in services)
            {
                var port = GetPort(service);
                dictionary[service.Key] = port;
            }
            return dictionary;
        }

        private int GetPort(IConfigurationSection section)
        {
            return int.TryParse(section["Port"], out var port) ? port : DefaultServicesPort;
        }

        public int GetPort(string jobName)
        {
            return _ports.TryGetValue(jobName, out var port) ? port : DefaultServicesPort;
        }
        
        private int ParseGrpcServerPort()
        {
            var result = _configuration["GrpcServer:Port"];
            return int.TryParse(result, out var port) ? port : DefaultGrpcServerPort;
        }


        private string ParseMetric(string metricType, string infoType)
        {
            var path = string.Format(MetricQueryPath, metricType, infoType);
            return _configuration[path];
        }

        private int ParseMetricTimeout()
        {
            var timeout = ParseMetric("default", "Timeout");
            if (int.TryParse(timeout, out var value) && value > 0)
            {
                return value * 1000;
            }

            return DefaultTimeout;
        }

        private string ParseMetricQuery(string metricType)
        {
            var path = _configuration["Metrics:default:BasePath"];
            var baseQuery = _configuration["Metrics:default:BaseQuery"];
            var query = ParseMetric(metricType, "Query");
            return $"{path}?{baseQuery}={query}";
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