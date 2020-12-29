using System;
using System.Net.Http;
using System.Threading.Tasks;
using Alley.Monitoring.Models;
using Alley.Utils;
using Newtonsoft.Json;

namespace Alley.Monitoring
{
    public abstract class MetricFetcherBase : IMetricFetcher
    {
        private readonly HttpClient _httpClient;
        private readonly string _query;
        private readonly IAlleyLogger _logger;

        protected MetricFetcherBase(HttpClient httpClient, string query, IAlleyLogger logger)
        {
            _httpClient = httpClient;
            _query = query;
            _logger = logger;
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
                _logger.Error(e.Message);
                throw;
            }
        }
    }
}