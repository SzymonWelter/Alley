using System;
using System.Collections.Generic;

namespace Alley.Context.Providers
{
    public interface IInstanceMetrics
    {
        Uri Address { get; }
        IDictionary<string, IInstanceMetric> Metrics{ get; }
    }
}