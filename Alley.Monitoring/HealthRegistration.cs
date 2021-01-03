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
        private readonly IMetricMetadataFactory _metadataFactory;
        private readonly IConfigurationProvider _configurationProvider;

        public HealthRegistration(
            IHealthFetcher healthFetcher,
            IContextManagement contextManagement,
            IMetricMetadataFactory metadataFactory,
            IConfigurationProvider configurationProvider)
        {
            _healthFetcher = healthFetcher;
            _contextManagement = contextManagement;
            _metadataFactory = metadataFactory;
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
                    if (_contextManagement.MicroserviceExists(instance.Job))
                    {
                        _contextManagement.RegisterInstance(instance.Job, new Uri(instance.Instance));
                    }
                }

                foreach (var instance in instancesToUnregister)
                {
                    _contextManagement.UnregisterInstance(new Uri(instance.Instance));
                }
            }
        }

        private IEnumerable<MetricMetadata> GetOldInstances()
        {
            return _contextManagement
                .GetInstances()
                .Select(_metadataFactory.GetFrom);
        }

        private IEnumerable<MetricMetadata> GetInstancesFromResult(MetricsSummary result)
        {
            return result.Data.Result
                .Where(m => (int) m.Value[1] == 1)
                .Select(_metadataFactory.GetFrom);
        }
    }
}