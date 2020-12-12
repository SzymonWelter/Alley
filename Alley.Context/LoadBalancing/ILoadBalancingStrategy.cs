using System;
using System.Collections.Generic;
using Alley.Utils.Models;

namespace Alley.Context.Providers
{
    public interface ILoadBalancingStrategy
    {
        Result<Uri> Execute(IEnumerable<IInstanceMetrics> serviceName);
    }
}