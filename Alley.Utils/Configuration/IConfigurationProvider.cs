using System.IO.Abstractions;

namespace Alley.Utils.Configuration
{
    public interface IConfigurationProvider
    {
        IDirectoryInfo GetProtosPath();
        string ProtoPattern { get; }
        string Protocol { get; }
        string CpuUsageQuery { get; }
        string HealthCheckQuery { get; }
        int HealthCheckTimeout { get;}
        int GrpcServerPort { get; }
        int MetricsTimeout { get; }
        int GetPort(string jobName);
    }
}