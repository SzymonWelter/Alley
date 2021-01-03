using System.Net.Http;
using Alley.Utils;
using Alley.Utils.Configuration;

namespace Alley.Monitoring
{
    public class CpuUsageFetcher : MetricFetcherBase
    {
        public CpuUsageFetcher(
            HttpClient httpClient, 
            IConfigurationProvider configurationProvider,
            IAlleyLogger logger) : 
            base(
                httpClient, 
                configurationProvider.CpuUsageQuery,
                logger)
        {
        }
    }
}