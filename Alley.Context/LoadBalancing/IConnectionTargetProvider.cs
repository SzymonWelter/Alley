using System;
using Alley.Utils.Models;

namespace Alley.Context.LoadBalancing
{
    public interface IConnectionTargetProvider
    {
        Result<Uri> GetTarget(string serviceName);
    }
}