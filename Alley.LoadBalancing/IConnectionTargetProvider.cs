using System;
using Alley.Utils.Models;

namespace Alley.LoadBalancing
{
    public interface IConnectionTargetProvider
    {
        Result<Uri> GetTarget(string serviceName);
    }
}