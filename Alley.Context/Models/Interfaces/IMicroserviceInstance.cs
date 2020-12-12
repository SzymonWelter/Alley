using System;
using System.Collections.Generic;
using Alley.Context.Metrics;
using Grpc.Net.Client;

namespace Alley.Context.Models.Interfaces
{
    public interface IMicroserviceInstance : IReadonlyMicroserviceInstance, IDisposable
    {
        GrpcChannel GetChannel();
        new IDictionary<MetricType, IInstanceMetric> Metrics{ get; }
    }

    public interface IReadonlyMicroserviceInstance
    {
        string MicroServiceName { get; }
        Uri Uri { get; }
        IEnumerable<KeyValuePair<MetricType, IInstanceMetric>> Metrics { get; }
    }
}