﻿using Alley.Context;
using Alley.Definitions.Mappers.Interfaces;
using Alley.LoadBalancing;
using Alley.Utils;
using Alley.Utils.Configuration;
using Alley.Utils.Helpers;
using Grpc.Core;

namespace Alley.Core.Handling
{
    public class SessionFactory<TRequest, TResponse> : ISessionFactory<TRequest, TResponse>
        where TRequest : class
        where TResponse : class
    {
        private readonly IConnectionTargetProvider _connectionTargetProvider;
        private readonly IMethodFactory<TRequest, TResponse> _methodFactory;
        private readonly IMetricRepository _metricRepository;
        private readonly IChannelProvider _channelProvider;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IAlleyLogger _logger;

        public SessionFactory(
            IConnectionTargetProvider connectionTargetProvider,
            IMethodFactory<TRequest, TResponse> methodFactory,
            IMetricRepository metricRepository, 
            IChannelProvider channelProvider,
            IConfigurationProvider configurationProvider,
            IAlleyLogger logger)
        {
            _connectionTargetProvider = connectionTargetProvider;
            _configurationProvider = configurationProvider;
            _methodFactory = methodFactory;
            _metricRepository = metricRepository;
            _channelProvider = channelProvider;
            _logger = logger;
        }

        public IConnectionSession<TRequest, TResponse> CreateSession(string methodFullName, MethodType methodType)
        {
            var method = _methodFactory.Create(methodFullName, methodType);

            var targetIpResult = _connectionTargetProvider.GetTarget(method.ServiceName);
            SessionHelper.HandleIfError(targetIpResult, _logger);

            var channelResult = _channelProvider.GetChannel(targetIpResult.Value);
            SessionHelper.HandleIfError(channelResult, _logger);

            return new ConnectionSession<TRequest, TResponse>(channelResult.Value, method, _metricRepository, _configurationProvider, _logger);
        }
    }
}