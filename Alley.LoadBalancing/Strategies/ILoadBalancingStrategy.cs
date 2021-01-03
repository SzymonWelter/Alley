using System;
using System.Collections.Generic;
using Alley.Context.Models.Interfaces;
using Alley.Utils.Models;

namespace Alley.LoadBalancing.Strategies
{
    public interface ILoadBalancingStrategy
    {
        Result<Uri> Execute(IEnumerable<IReadonlyMicroserviceInstance> instances);
    }
}