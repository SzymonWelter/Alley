using System;
using Alley.Utils.Models;

namespace Alley.Context.Providers
{
    public interface ILoadBalancingExecutor
    {
        Result<Uri> GetTarget(string serviceName);
    }
}