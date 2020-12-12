using System;
using Alley.Context.Providers;

namespace Alley.Context.LoadBalancing
{
    public interface IMetricRepository
    {
        void UpdateMetric(Uri uri, string metricName, Func<IInstanceMetric, IInstanceMetric> newMetricRecipe);
    }
}