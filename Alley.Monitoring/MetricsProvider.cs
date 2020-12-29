using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alley.Monitoring.Models;

namespace Alley.Monitoring
{
    public class MetricsProvider : IMetricsProvider
    {
        private readonly IEnumerable<IMetricFetcher> _fetchers;

        public MetricsProvider(IFetchersProvider fetchersProvider)
        {
            _fetchers = fetchersProvider.GetFetchers();
        }
        public async Task<IEnumerable<MetricsSummary>> GetMetrics()
        {
            var tasks = _fetchers.Select(f => f.Fetch());
            var result = await Task.WhenAll(tasks);
            return result;
        }
    }
}