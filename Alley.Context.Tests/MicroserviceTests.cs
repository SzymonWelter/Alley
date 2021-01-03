using System;
using System.Collections.Generic;
using Alley.Context.Metrics;
using Alley.Context.Models;
using Alley.Context.Models.Interfaces;
using Alley.Utils;
using Xunit;

namespace Alley.Context.Tests
{
    public class MicroserviceTests
    {
        private Microservice _sut;
        private readonly IDictionary<Uri, IMicroserviceInstance> _instances;
        private Dictionary<MetricType, IInstanceMetric> _metricDictionary;
        private IEnumerable<string> _servicesNames;
        private const string MicroserviceName = "microservice";

        public MicroserviceTests()
        {
            _instances = new Dictionary<Uri, IMicroserviceInstance>();
            _metricDictionary = new Dictionary<MetricType, IInstanceMetric>();
            _servicesNames = new List<string>();
            _sut = new Microservice(MicroserviceName,_servicesNames, _instances);
        }

        [Fact]
        public void When_passed_parameters_to_constructor_then_correct_values_are_stored()
        {
            // Assert
            Assert.Equal(_sut.Name, MicroserviceName);
            Assert.Equal(_sut.GetInstances(), _instances.Values);
        }
        
        [Fact]
        public void When_passed_name_to_constructor_then_instances_should_be_empty()
        {
            // Act
            _sut = new Microservice(MicroserviceName);
            
            // Assert
            Assert.NotNull(_sut.GetInstances());
            Assert.Empty(_sut.GetInstances());
        }

        [Fact]
        public void When_register_instance_with_null_uri_then_registration_should_return_failure_result()
        {
            // Arrange
            var uri = (Uri) null;
            var instance = new MicroserviceInstance(MicroserviceName, uri, _metricDictionary);

            // Act
            var result = _sut.RegisterInstance(instance);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(Messages.CanNotRegisterInstance(instance), result.Message);
        }
        
        [Fact]
        public void When_register_null_then_registration_should_return_failure_result()
        {
            // Act
            var result = _sut.RegisterInstance(null);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(Messages.CanNotRegisterInstance(null), result.Message);
        }
        
        [Fact]
        public void When_register_correct_instance_then_registration_should_return_success_result()
        {
            // Arrange
            var uri = new Uri("http://test");
            var instance = new MicroserviceInstance(MicroserviceName, uri, _metricDictionary);
            
            // Act
            var result = _sut.RegisterInstance(instance);
            var instances = _sut.GetInstances();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEmpty(instances);
            Assert.Contains(instance, instances);
            Assert.Equal(Messages.InstanceSuccessfullyRegistered(instance), result.Message);
        }

        [Fact]
        public void When_unregister_not_existing_instance_then_result_is_failure()
        {
            // Arrange
            var uri = new Uri("http://test");

            // Act
            var result = _sut.UnregisterInstance(uri);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(Messages.CanNotUnregisterNotExistingInstance(uri, MicroserviceName), result.Message);
        }

        [Fact]
        public void When_registered_instance_and_unregister_then_result_of_should_be_success()
        {
            // Arrange
            var uri = new Uri("http://test");
            var instance = new MicroserviceInstance(MicroserviceName,uri, _metricDictionary);

            // Act
            var registrationResult = _sut.RegisterInstance(instance);
            var unregistrationResult = _sut.UnregisterInstance(instance.Uri);

            // Assert
            Assert.True(registrationResult.IsSuccess);
            Assert.True(unregistrationResult.IsSuccess);

            Assert.Equal(Messages.InstanceSuccessfullyRegistered(instance), registrationResult.Message);
            Assert.Equal(Messages.InstanceSuccessfullyUnregistered(instance), unregistrationResult.Message);
        }
        
        [Fact]
        public void When_unregister_null_then_registration_should_return_failure_result()
        {
            // Act
            var result = _sut.UnregisterInstance(null);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(Messages.CanNotUnregisterInstanceWithNullUri(MicroserviceName), result.Message);
        }
    }
}