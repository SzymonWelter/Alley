using System;
using System.Threading.Tasks;
using Alley.Context;
using Alley.Context.Metrics;
using Alley.Monitoring.Models;
using Alley.Utils.Configuration;

namespace Alley.Monitoring
{
    public class MetricsRegistration : IMetricsRegistration
    {
        private readonly IMetricsProvider _metricsProvider;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IMetricRepository _metricRepository;
        private readonly IMetricMetadataFactory _metadataFactory;

        public MetricsRegistration(
            IMetricsProvider metricsProvider, 
            IConfigurationProvider configurationProvider, 
            IMetricRepository metricRepository,
            IMetricMetadataFactory metadataFactory)
        {
            _metricsProvider = metricsProvider;
            _configurationProvider = configurationProvider;
            _metricRepository = metricRepository;
            _metadataFactory = metadataFactory;
        }
        public async Task Start()
        {
            while (true)
            {
                await Task.Delay(_configurationProvider.MetricsTimeout);
                var metrics = await _metricsProvider.GetMetrics();
                foreach (var metric in metrics)
                {
                    var result = metric.Data;
                    foreach (var metricResult in result.Result)
                    {
                        var metadata = _metadataFactory.GetFrom(metricResult);
                        _metricRepository.AddOrUpdateMetric(new Uri(metadata.Instance), new MemoryUsageMetric(metricResult.Value[1]));
                    }
                }
            }
        }
    }
}