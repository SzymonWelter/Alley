using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alley.Context;
using Alley.Monitoring.Models;
using Alley.Utils.Configuration;

namespace Alley.Monitoring
{
    public class HealthRegistration : IHealthRegistration
    {
        private readonly IHealthFetcher _healthFetcher;
        private readonly IContextManagement _contextManagement;
        private readonly IConfigurationProvider _configurationProvider;

        public HealthRegistration(
            IHealthFetcher healthFetcher, 
            IContextManagement contextManagement, 
            IConfigurationProvider configurationProvider)
        {
            _healthFetcher = healthFetcher;
            _contextManagement = contextManagement;
            _configurationProvider = configurationProvider;
        }
        public async Task Start()
        {
            while (true)
            {
                await Task.Delay(_configurationProvider.HealthCheckTimeout);
                
                var oldInstances = GetOldInstances();
                var result = await _healthFetcher.Fetch();
                var currentInstances = GetInstancesFromResult(result);
                var instancesToRegister = currentInstances.Except(oldInstances);
                var instancesToUnregister = oldInstances.Except(currentInstances);

                foreach (var instance in instancesToRegister)
                {
                    if(_contextManagement.MicroserviceExists(instance.Job))
                        _contextManagement.RegisterInstance(instance.Job, GetUri(instance.Instance));
                }

                foreach (var instance in instancesToUnregister)
                {
                    _contextManagement.UnregisterInstance(GetUri(instance.Instance));
                }
            }
        }
        private static Uri GetUri(string instance)
        {
            return new Uri(instance);
        }

        private IEnumerable<MetricMetadata> GetOldInstances()
        {
            return _contextManagement.GetInstances().Select(i => new MetricMetadata
            {
                Instance = i.Uri.OriginalString,
                Job = i.MicroServiceName
            });
        }

        private IEnumerable<MetricMetadata> GetInstancesFromResult(MetricsSummary result)
        {
            return result.Data.Result
                .Where(m => (int)m.Value[1] == 1)
                .Select(m =>
                {
                    var instance = FormatInstance(m.Metric.Instance, m.Metric.Job);
                    return new MetricMetadata
                    {
                        Instance = instance,
                        Job = m.Metric.Job
                    };
                });
        }

        private string FormatInstance(string metricInstance, string jobName)
        {
            var uri = new Uri($"{_configurationProvider.Protocol}://{metricInstance}");
            return $"{uri.Scheme}://{uri.Host}:{_configurationProvider.GetPort(jobName)}";
        }

    }
}