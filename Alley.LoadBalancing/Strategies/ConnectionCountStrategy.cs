using System;
using System.Collections.Generic;
using System.Linq;
using Alley.Context.Metrics;
using Alley.Context.Models.Interfaces;
using Alley.Utils;
using Alley.Utils.Models;

namespace Alley.LoadBalancing.Strategies
{
    public class ConnectionCountStrategy : ILoadBalancingStrategy
    {
        public Result<Uri> Execute(IEnumerable<IReadonlyMicroserviceInstance> instances)
        {
            if (instances == null || !instances.Any())
            {
                return Result<Uri>.Failure(Messages.CanNotExecuteLoadBalancingStrategyForZeroAvailableInstances());
            }
            var result = instances.OrderBy(
                i => i.Metrics.First(
                    m => m.Key == MetricType.ActiveConnection).Value);
            return Result<Uri>.Success(result.First().Uri);
        }
    }
}