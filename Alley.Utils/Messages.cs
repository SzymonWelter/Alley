using System;
using System.Collections.Generic;
using Alley.Utils.Helpers;

namespace Alley.Utils
{
    public static class Messages
    {
        public static string FileCanNotBeAddedToDescriptor(string fileName) =>  
            $"Can not add {fileName} to descriptor.";
        public static string FileHasBeenAddedToDescriptor(string fileName) =>  
            $"File {fileName} has been added to descriptor.";

        public static string MicroserviceDoesntExistMessage(string microserviceName) =>
            $"Microservice: ${microserviceName} doesnt exist";

        public static string ServiceDoesntExistMessage (string serviceName) =>
            $"Service:  ${serviceName} doesnt exist";

        public static string ServiceHasBeenAlreadyRegisteredMessage(string serviceName) =>
            $"Can not register service: {serviceName} has been already registered\n";

        public static string MicroserviceHasBeenAlreadyRegisteredMessage(string microserviceName) =>
            $"Can not register microservice: {microserviceName} has been already registered";

        public static string MicroserviceHasBeenRegistered(string microserviceName) =>
            $"Microservice: {microserviceName} has been registered";

        public static string MetricHasBeenUpdated(object metric) =>
            $"Metric: {metric} has been updated";

        public static string InstanceDoesntExistMessage(Uri uri) =>
            $"Instance: {uri} doesnt exist";

        public static string CanNotUpdateMetric(string metricName) =>
            $"Can not update metric: {metricName} based on previous metric because it doesnt exist";

        public static string InstanceSuccessfullyRegistered(object instance) =>
            $"Instance: {instance} successfully registered";

        public static string InstanceAlreadyRegistered(string microserviceName, Uri uri) =>
            $"Instance:{FormatHelper.FormatMicroserviceInstance(microserviceName, uri)} already registered before";

        public static string InstancesOfMicroserviceDoesntExist(string microserviceName) =>
            $"There is no instance of {microserviceName} microservice";

        public static string CanNotRegisterInstance(object microServiceInstance) =>
            $"Can not register {microServiceInstance} instance";

        public static string CanNotUnregisterInstanceWithNullUri(string serviceName)=>
            $"Can not unregister instance of {serviceName} microservice with null uri";
        
        public static string CanNotUnregisterInstanceWithNullUri() =>
            $"Can not unregister instance with null uri";

        public static string InstanceSuccessfullyUnregistered(object instance) =>
            $"Instance: {instance} successfully unregistered";

        public static string CanNotUnregisterNotExistingInstance(Uri uri, string microserviceName) =>
            $"Can not unregister not existing instance: {uri} of {microserviceName} microservice";

        public static string CanNotCompareMetrics(object metric, object otherMetric) =>
            $"Can not compare metric with different types: {metric} | {otherMetric}";

        public static string MicroserviceNameCanNotBeNullOrEmptyOrWhitespace() =>
            "Microservice name can not be null or empty or whitespace";

        public static string CanNotRegisterMicroserviceWithoutServices(string microserviceName) =>
            $"Microservice: {microserviceName} Can not register microservice without services";

        public static string CanNotRegisterInstanceWithNullUri(string microserviceName) =>
            $"Can not register {microserviceName} instance with null uri";

        public static string CanNotExecuteLoadBalancingStrategyForZeroAvailableInstances() =>
            "Can not execute load balancing strategy for zero available instances";

        public static string CanNotFindSuitableTarget(IEnumerable<object> instances) =>
            $"Can not find suitable targets: {string.Join(" | ", instances)}";

        public static string ConnectionStartedWith(string target) =>
            $"Connection started with {target}";

        public static string ConnectionEndedWith(string target) =>
            $"Connection ended with {target}";

        public static string MonitoringCrashed(string eMessage) =>
            $"Monitoring crashed: {eMessage}";
    }
}