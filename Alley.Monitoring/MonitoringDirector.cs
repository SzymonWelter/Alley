using System;
using System.Threading.Tasks;
using Alley.Utils;

namespace Alley.Monitoring
{
    public class MonitoringDirector : IMonitoringDirector
    {
        private readonly IHealthRegistration _healthRegistration;
        private readonly IMetricsRegistration _metricsRegistration;
        private readonly IAlleyLogger _logger;

        public MonitoringDirector(
            IHealthRegistration healthRegistration, 
            IMetricsRegistration metricsRegistration, 
            IAlleyLogger logger)
        {
            _healthRegistration = healthRegistration;
            _metricsRegistration = metricsRegistration;
            _logger = logger;
        }

        public void Run()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        private async Task RunAsync()
        {
            while (true)
            {
                try
                {
                    var healthTask = _healthRegistration.Start();
                    var metricsTask = _metricsRegistration.Start();
                    await Task.WhenAny(healthTask, metricsTask);
                }
                catch (Exception e)
                {
                    _logger.Error(Messages.MonitoringCrashed(e.Message));
                }
            }
        }
    }
}