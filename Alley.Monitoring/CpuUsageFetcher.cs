using System.Net.Http;
using Alley.Utils.Configuration;

namespace Alley.Monitoring
{
    public class CpuUsageFetcher : MetricFetcherBase
    {
        public CpuUsageFetcher(
            HttpClient httpClient, 
            IConfigurationProvider configurationProvider) : 
            base(
                httpClient, 
                configurationProvider.CpuUsageQuery)
        {
        }
    }
}