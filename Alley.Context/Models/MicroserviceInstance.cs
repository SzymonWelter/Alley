using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Alley.Context.Metrics;
using Alley.Context.Models.Interfaces;
using Alley.Utils.Helpers;
using Grpc.Net.Client;

namespace Alley.Context.Models
{
    public class MicroserviceInstance : IMicroserviceInstance
    {
        public string MicroServiceName { get; }
        public Uri Uri { get; }
        public IDictionary<MetricType, IInstanceMetric> Metrics { get; }
        IEnumerable<KeyValuePair<MetricType, IInstanceMetric>> IReadonlyMicroserviceInstance.Metrics => Metrics;
        private GrpcChannel _channel;

        public MicroserviceInstance(string microServiceName, Uri uri) : 
            this(microServiceName, uri, new Dictionary<MetricType, IInstanceMetric>()){}
        public MicroserviceInstance(string microServiceName, Uri uri, IDictionary<MetricType, IInstanceMetric> metrics)
        {
            MicroServiceName = microServiceName;
            Uri = uri;
            Metrics = metrics ?? new ConcurrentDictionary<MetricType, IInstanceMetric>();
        }
        
        public GrpcChannel GetChannel()
        {
            return _channel ??= GrpcChannel.ForAddress(Uri);
        }

        public override string ToString()
        {
            return FormatHelper.FormatMicroserviceInstance(MicroServiceName, Uri);
        }

        public void Dispose()
        {
            _channel?.Dispose();
        }
    }
}