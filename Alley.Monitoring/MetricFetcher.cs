namespace Alley.Monitoring
{
    public class MetricFetcher : IMetricFetcher
    {
        public void Run()
        {
        }
    }

    public interface IMetricFetcher
    {
        void Run();
    }
}