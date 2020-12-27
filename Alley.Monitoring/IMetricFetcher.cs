using System.Threading.Tasks;
using Alley.Monitoring.Models;

namespace Alley.Monitoring
{
    public interface IMetricFetcher
    {
        Task<MetricsSummary> Fetch();
    }
}