using System;
using System.Collections.Generic;
using System.Linq;
using Alley.Context.Metrics;
using Alley.Context.Models.Interfaces;
using Alley.Utils;
using Alley.Utils.Models;

namespace Alley.LoadBalancing.Strategies
{
    public class ActiveConnectionCountStrategy : ILoadBalancingStrategy
    {
        public Result<Uri> Execute(IEnumerable<IReadonlyMicroserviceInstance> instances)
        {
            if (instances == null || !instances.Any())
            {
                return Result<Uri>.Failure(Messages.CanNotExecuteLoadBalancingStrategyForZeroAvailableInstances());
            }
            
            IInstanceMetric bestMetric = new ActiveConnectionMetric(double.MaxValue);
            IReadonlyMicroserviceInstance bestInstance = null;
            
            foreach (var instance in instances)
            {
                if (TryGetActiveConnectionMetric(instance, out var metric))
                {
                    if (bestMetric.CompareTo(metric) >= 0)
                    {
                        bestMetric = metric;
                        bestInstance = instance;
                    }
                }
            }

            return bestInstance == null ? 
                Result<Uri>.Failure(Messages.CanNotFindSuitableTarget(instances)) :
                Result<Uri>.Success(bestInstance.Uri);
        }

        private static bool TryGetActiveConnectionMetric(IReadonlyMicroserviceInstance instance, out IInstanceMetric? metric)
        {
            if (instance?.Metrics == null)
            {
                metric = null;
                return false;
            }

            metric = instance.Metrics
                .FirstOrDefault(m => 
                    m.Key == MetricType.ActiveConnection)
                .Value;
            return metric != null;
        }
    }
}