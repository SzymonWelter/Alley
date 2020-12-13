using System;
using System.Collections.Generic;
using Alley.Context.Metrics;
using Alley.Context.Models.Interfaces;
using Alley.Utils.Models;
using Grpc.Core;

namespace Alley.Context
{
    public interface IMicroserviceContext : IContextManagement, IReadonlyInstanceContext, IChannelProvider
    {
    }

    public interface IContextManagement : IMetricRepository
    {
        IResult RegisterMicroservice(string microserviceName, IEnumerable<string> servicesNames);
        IResult UnregisterMicroservice(string microserviceName);

        IResult RegisterInstance(string microserviceName, Uri microserviceInstance);
        IResult UnregisterInstance(Uri instanceUri);
        
    }

    public interface IReadonlyInstanceContext
    {
        IResult<IEnumerable<IReadonlyMicroserviceInstance>> GetInstances(string serviceName);
    }

    public interface IMetricRepository
    {
        IResult UpdateMetric(Uri uri, MetricType metricType, Func<IInstanceMetric, IInstanceMetric> updateRecipe);
    }

    public interface IChannelProvider
    {
        IResult<ChannelBase> GetChannel(Uri uri);
    }
}