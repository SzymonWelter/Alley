using System.Collections.Generic;
using System.Threading.Tasks;
using Alley.Monitoring.Models;

namespace Alley.Monitoring
{
    public interface IMetricsProvider
    {
        Task<IEnumerable<MetricsSummary>> GetMetrics();
    }
}