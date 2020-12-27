using System.Net.Http;
using System.Threading.Tasks;
using Alley.Monitoring.Models;
using Newtonsoft.Json;

namespace Alley.Monitoring
{
    public abstract class MetricFetcherBase : IMetricFetcher
    {
        private HttpClient _httpClient;

        protected MetricFetcherBase(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public abstract Task<MetricsSummary> Fetch();

        protected virtual async Task<MetricsSummary> Fetch(string query)
        {
            var result = await _httpClient.GetStringAsync(query);
            return JsonConvert.DeserializeObject<MetricsSummary>(result);
        }
    }
}