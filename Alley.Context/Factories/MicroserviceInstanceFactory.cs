using System;
using System.Collections.Concurrent;
using Alley.Context.Metrics;
using Alley.Context.Models;
using Alley.Context.Models.Interfaces;

namespace Alley.Context.Factories
{
    public class MicroserviceInstanceFactory : IMicroserviceInstanceFactory
    {
        public IMicroserviceInstance Create(string microserviceName, Uri uri)
        {
            var metricDictionary = CreateMetricDictionary();
            return new MicroserviceInstance(microserviceName, uri, metricDictionary);
        }

        private static ConcurrentDictionary<MetricType, IInstanceMetric> CreateMetricDictionary()
        {
            var metricDictionary = new ConcurrentDictionary<MetricType, IInstanceMetric>();
            metricDictionary.TryAdd(MetricType.ActiveConnection, new ActiveConnectionMetric(0));
            return metricDictionary;
        }
    }
}