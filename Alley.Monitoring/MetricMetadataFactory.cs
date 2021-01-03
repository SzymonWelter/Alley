using System;
using Alley.Context.Models.Interfaces;
using Alley.Monitoring.Models;
using Alley.Utils.Configuration;

namespace Alley.Monitoring
{
    public class MetricMetadataFactory : IMetricMetadataFactory
    {
        private readonly IConfigurationProvider _configurationProvider;
        public MetricMetadataFactory(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }
        public MetricMetadata GetFrom(MetricResult metricResult)
        {
            var instance = FormatUri(metricResult.Metric.Instance, metricResult.Metric.Job);
            return new MetricMetadata
            {
                Instance = instance,
                Job = metricResult.Metric.Job
            };
        }

        public MetricMetadata GetFrom(IReadonlyMicroserviceInstance microserviceInstance)
        {
            return new MetricMetadata
            {
                Instance = microserviceInstance.Uri.OriginalString,
                Job = microserviceInstance.MicroServiceName
            };
        }
        
        private string FormatUri(string metricInstance, string jobName)
        {
            var uri = new Uri($"{_configurationProvider.Protocol}://{metricInstance}");
            return $"{uri.Scheme}://{uri.Host}:{_configurationProvider.GetPort(jobName)}";
        }
    }
}