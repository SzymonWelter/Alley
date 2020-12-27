using System.Threading.Tasks;

namespace Alley.Monitoring
{
    public class MetricFetcher : IMetricFetcher
    {
        public MetricFetcher()
        {
            
        }

        public void Run()
        {
            Task.Run(async () =>
            {
                while (true)
                {

                }
            }).GetAwaiter().GetResult();
        }
    }
    
    public interface IMetricFetcher
    {
        void Run();
    }
}