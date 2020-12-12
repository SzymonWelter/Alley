using System;
using System.Collections.Generic;
using System.Linq;
using Alley.Context.Providers;
using Alley.Utils.Models;

namespace Alley.Context.LoadBalancing
{
    public class LoadBalancingDirector : ILoadBalancingExecutor, IMetricRepository
    {
        private readonly ILoadBalancingStrategy _strategy;
        private readonly IReadonlyMicroserviceRepository _microserviceRepository;
        private readonly IDictionary<Uri, IInstanceMetrics> _instanceMetrics;

        public LoadBalancingDirector(ILoadBalancingStrategy strategy, IReadonlyMicroserviceRepository microserviceRepository) : 
            this(strategy, microserviceRepository, new Dictionary<Uri, IInstanceMetrics>())
        { }

        public LoadBalancingDirector(ILoadBalancingStrategy strategy, IReadonlyMicroserviceRepository microserviceRepository, IDictionary<Uri, IInstanceMetrics> instanceMetrics)
        {
            _strategy = strategy;
            _microserviceRepository = microserviceRepository;
            _instanceMetrics = instanceMetrics;
        }

        public Result<Uri> GetTarget(string serviceName)
        {
            var instancesResult = _microserviceRepository.GetInstances(serviceName);
            if (instancesResult.IsFailure)
            {
                return Result<Uri>.Failure(instancesResult.Message);
            }

            var candidates = instancesResult.Value.Select(i => _instanceMetrics[i.Uri]);
            var result = _strategy.Execute(candidates);
            return result;
        }

        public void UpdateMetric(Uri uri, string metricName, Func<IInstanceMetric, IInstanceMetric> newMetricRecipe)
        {
            var metrics = _instanceMetrics[uri];
            var metric = metrics.Metrics[metricName];
            metrics.Metrics[metricName] = newMetricRecipe(metric);
        }
    }
}