using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Alley.Context.Factories;
using Alley.Context.Metrics;
using Alley.Context.Models;
using Alley.Context.Models.Interfaces;
using Alley.Utils;
using Alley.Utils.Models;
using Grpc.Core;

namespace Alley.Context
{
    public class MicroserviceContext : IMicroserviceContext
    {
        private readonly IDictionary<string, IMicroservice> _servicesOwners;
        private readonly IDictionary<string, IMicroservice> _microservices;
        private readonly IDictionary<Uri, IMicroserviceInstance> _instances;
        private readonly IMicroserviceInstanceFactory _microserviceInstanceFactory;
        private readonly IAlleyLogger _logger;

        public MicroserviceContext(
            IMicroserviceInstanceFactory microserviceInstanceFactory,
            IAlleyLogger logger) : this(
            new ConcurrentDictionary<string, IMicroservice>(), 
            new ConcurrentDictionary<string, IMicroservice>(), 
            new ConcurrentDictionary<Uri, IMicroserviceInstance>(),
            microserviceInstanceFactory,
            logger)
        { }

        public MicroserviceContext(
            IDictionary<string, IMicroservice> servicesOwners, 
            IDictionary<string, IMicroservice> microservices, 
            IDictionary<Uri, IMicroserviceInstance> instances,
            IMicroserviceInstanceFactory microserviceInstanceFactory,
            IAlleyLogger logger)
        {
            _servicesOwners = servicesOwners;
            _microservices = microservices;
            _instances = instances;
            _microserviceInstanceFactory = microserviceInstanceFactory;
            _logger = logger;
        }

        public IResult<IEnumerable<IReadonlyMicroserviceInstance>> GetInstances(string serviceName)
        {
            if (!_servicesOwners.TryGetValue(serviceName, out var microservice))
            {
                return Result<IEnumerable<IReadonlyMicroserviceInstance>>.Failure(Messages.ServiceDoesntExistMessage(serviceName));
            }

            var instances = microservice.GetInstances();
            return instances != null && instances.Any() ?
                Result<IEnumerable<IReadonlyMicroserviceInstance>>.Success(instances) :
                Result<IEnumerable<IReadonlyMicroserviceInstance>>.Failure(Messages.InstancesOfMicroserviceDoesntExist(microservice.Name));
        }

        public IEnumerable<IReadonlyMicroserviceInstance> GetInstances()
        {
            return _instances.Values;
        }

        public IResult UpdateMetric(Uri uri, MetricType metricType, Func<IInstanceMetric, IInstanceMetric> updateRecipe)
        {
            if (!_instances.TryGetValue(uri, out var instance))
            {
                return Result.Failure(Messages.InstanceDoesntExistMessage(uri));
            }

            if (!instance.Metrics.TryGetValue(metricType, out var metric))
            {
                return Result.Failure(Messages.CanNotUpdateMetric(metricType.ToString()));
            }
            UpdateMetricInternal(instance.Metrics, metricType, updateRecipe(metric));
            
            return Result.Success(Messages.MetricHasBeenUpdated(instance.Metrics[metricType]));
        }
        
        public IResult AddOrUpdateMetric(Uri uri, IInstanceMetric metric)
        {
            if (!_instances.TryGetValue(uri, out var instance))
            {
                return Result.Failure(Messages.InstanceDoesntExistMessage(uri));
            }
            UpdateMetricInternal(instance.Metrics, metric.Type, metric);
            
            return Result.Success(Messages.MetricHasBeenUpdated(metric));
        }

        public IResult RegisterMicroservice(string microserviceName, IEnumerable<string> servicesNames)
        {
            if (string.IsNullOrEmpty(microserviceName) || string.IsNullOrWhiteSpace(microserviceName))
            {
                var failureMessage = Messages.MicroserviceNameCanNotBeNullOrEmptyOrWhitespace();
                _logger.Error(failureMessage);
                return Result.Failure(failureMessage);
            }

            if (servicesNames == null || !servicesNames.Any())
            {
                var failureMessage = Messages.CanNotRegisterMicroserviceWithoutServices(microserviceName);
                _logger.Error(failureMessage);
                return Result.Failure(failureMessage);
            }
            
            var microservice = new Microservice(microserviceName, servicesNames);
            if (!_microservices.TryAdd(microserviceName, microservice))
            {
                var failureMessage = Messages.MicroserviceHasBeenAlreadyRegisteredMessage(microserviceName);
                _logger.Error(failureMessage);
                return Result.Failure(failureMessage);
            }

            var message = servicesNames
                .Where(serviceName => !_servicesOwners.TryAdd(serviceName, microservice))
                .Aggregate(string.Empty, (current, serviceName) => 
                        current + Messages.ServiceHasBeenAlreadyRegisteredMessage(serviceName));

            var result =  string.IsNullOrEmpty(message) ? 
                Result.Success(Messages.MicroserviceHasBeenRegistered(microserviceName)) : 
                Result.Failure(message);
            
            _logger.LogResult(result);

            return result;
        }

        public bool MicroserviceExists(string microserviceName)
        {
            return _microservices.ContainsKey(microserviceName);
        }

        public IResult UnregisterMicroservice(string microserviceName)
        {
            if (microserviceName == null)
            {
                return Result.Failure(Messages.MicroserviceNameCanNotBeNullOrEmptyOrWhitespace());
            }
            
            if (_microservices.Remove(microserviceName, out var removedMicroservice))
            {
                return Result.Failure(Messages.MicroserviceDoesntExistMessage(microserviceName));
            }
            var urisToRemove = removedMicroservice.UnregisterAllInstances();
            foreach (var uri in urisToRemove.Value)
            {
                _instances.Remove(uri);
            }

            foreach (var serviceName in removedMicroservice.ServiceNames)
            {
                _servicesOwners.Remove(serviceName);
            }

            return Result.Success();
        }

        public IResult RegisterInstance(string microserviceName, Uri uri)
        {
            if (uri == null)
            {
                var message = Messages.CanNotRegisterInstanceWithNullUri(microserviceName);
                _logger.Error(message);
                return Result.Failure(message);
            }
            if (_instances.ContainsKey(uri))
            {
                var message = Messages.InstanceAlreadyRegistered(microserviceName, uri);
                _logger.Error(message);
                return Result.Failure(message);
            }

            if (!_microservices.TryGetValue(microserviceName, out var microservice))
            {
                var message = Messages.MicroserviceDoesntExistMessage(microserviceName);
                _logger.Error(message);
                return Result.Failure(message);
            }

            var instance = _microserviceInstanceFactory.Create(microserviceName, uri);
            var result = microservice.RegisterInstance(instance);
            if (result.IsSuccess)
            {
                _instances[uri] = instance;
            }
            _logger.LogResult(result);
            return result;

        }

        public IResult UnregisterInstance(Uri instanceUri)
        {
            if (instanceUri == null)
            {
                return Result.Failure(Messages.CanNotUnregisterInstanceWithNullUri());
            }
            if (!_instances.Remove(instanceUri, out var removedInstance))
            {
                return Result.Failure(Messages.InstanceDoesntExistMessage(instanceUri));
            }

            if (!_microservices.TryGetValue(removedInstance.MicroServiceName, out var microservice))
            {
                return Result.Failure(Messages.MicroserviceDoesntExistMessage(removedInstance.MicroServiceName));
            }
            var result = microservice.UnregisterInstance(instanceUri);
            _logger.LogResult(result);
            return result;
        }

        public IResult<ChannelBase> GetChannel(Uri uri)
        {
            return _instances.TryGetValue(uri, out var instance)
                ? Result<ChannelBase>.Success(instance.GetChannel())
                : Result<ChannelBase>.Failure(Messages.InstanceDoesntExistMessage(uri));
        }
        
        private static void UpdateMetricInternal(IDictionary<MetricType, IInstanceMetric> metrics, MetricType metricType, IInstanceMetric metric)
        {
            metrics[metricType] = metric;
        }
    }
}