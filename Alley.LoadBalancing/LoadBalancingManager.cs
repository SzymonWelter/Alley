using System;
using Alley.Context;
using Alley.LoadBalancing.Strategies;
using Alley.Utils.Models;

namespace Alley.LoadBalancing
{
    public class LoadBalancingManager : IConnectionTargetProvider
    {
        private readonly ILoadBalancingStrategy _strategy;
        private readonly IReadonlyInstanceContext _instanceContext;

        public LoadBalancingManager(
            IReadonlyInstanceContext instanceContext, 
            ILoadBalancingStrategy strategy)
        {
            _instanceContext = instanceContext;
            _strategy = strategy;
        }
        
        public Result<Uri> GetTarget(string serviceName)
        {
            var instancesResult = _instanceContext.GetInstances(serviceName);
            if (instancesResult.IsFailure)
            {
                return Result<Uri>.Failure(instancesResult.Message);
            }
            var result = _strategy.Execute(instancesResult.Value);
            return result;
        }
    }
}