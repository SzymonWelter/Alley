using System;
using System.Net.Http;
using System.Threading.Tasks;
using Alley.Monitoring.Models;
using Newtonsoft.Json;

namespace Alley.Monitoring
{
    public abstract class MetricFetcherBase : IMetricFetcher
    {
        private readonly HttpClient _httpClient;
        private readonly string _query;

        protected MetricFetcherBase(HttpClient httpClient, string query)
        {
            _httpClient = httpClient;
            _query = query;
        }

        public virtual async Task<MetricsSummary> Fetch()
        {
            try
            {
                var result = await _httpClient.GetStringAsync(_query);
                return JsonConvert.DeserializeObject<MetricsSummary>(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}