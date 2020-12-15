using System;
using System.Collections.Generic;
using System.Linq;
using Alley.Context.Factories;
using Alley.Context.Models;
using Alley.Context.Models.Interfaces;
using Alley.Utils;
using Alley.Utils.Models;
using NSubstitute;
using Xunit;

namespace Alley.Context.Tests
{
    public class MicroserviceContextTests
    {
        private readonly IAlleyLogger _logger;
        private readonly IMicroserviceInstanceFactory _microserviceInstanceFactory;
        private readonly IDictionary<string, IMicroservice> _servicesOwners;
        private readonly IDictionary<string, IMicroservice> _microservices;
        private readonly IDictionary<Uri, IMicroserviceInstance> _instances;
        private MicroserviceContext _sut;

        public MicroserviceContextTests()
        {
            _logger = Substitute.For<IAlleyLogger>();
            _microserviceInstanceFactory = Substitute.For<IMicroserviceInstanceFactory>();
            
            _servicesOwners = Substitute.For<IDictionary<string, IMicroservice>>();
            _microservices = Substitute.For<IDictionary<string, IMicroservice>>();
            _instances = Substitute.For<IDictionary<Uri, IMicroserviceInstance>>();
            _sut = new MicroserviceContext(
                _servicesOwners,
                _microservices,
                _instances,
                _microserviceInstanceFactory,
                _logger);
        }

        [Fact]
        public void When_get_instances_and_service_not_exist_then_result_is_failure()
        {
            // Arrange
            var serviceName = "testService";
            var expectedMessage = Messages.ServiceDoesntExistMessage(serviceName);
            _servicesOwners.TryGetValue(default, out _).ReturnsForAnyArgs(false);

            // Act
            var result = _sut.GetInstances(serviceName);

            // Assert
            _servicesOwners.ReceivedWithAnyArgs().TryGetValue(default, out _);
            Assert.True(result.IsFailure);
            Assert.Equal(expectedMessage, result.Message);
        }

        [Fact]
        public void When_successfully_get_instances_then_result_is_success()
        {
            // Arrange
            var serviceName = "testService";
            var enumerator = Substitute.For<IEnumerator<IReadonlyMicroserviceInstance>>();
            enumerator.MoveNext().Returns(true);
            var microservice = Substitute.For<IMicroservice>();
            var instances = Substitute.For<IEnumerable<IReadonlyMicroserviceInstance>>();
            instances.GetEnumerator().Returns(enumerator);
            microservice.GetInstances().Returns(instances);
            _servicesOwners.TryGetValue(default, out _).ReturnsForAnyArgs(x =>
            {
                x[1] = microservice;
                return true;
            });

            // Act
            var result = _sut.GetInstances(serviceName);

            // Assert
            _servicesOwners.ReceivedWithAnyArgs().TryGetValue(default, out _);
            Assert.True(result.IsSuccess);
            Assert.Equal(instances, result.Value);
            Assert.Empty(result.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData]
        public void When_register_microservice_with_null_or_empty_enumerable_then_result_is_failure(params string [] services)
        {
            // Arrange
            var microserviceName = "test";
            var expectedMessage = Messages.CanNotRegisterMicroserviceWithoutServices(microserviceName);
            // Act
            var result = _sut.RegisterMicroservice(microserviceName, services);

            // Assert
            _microservices.DidNotReceiveWithAnyArgs().TryAdd(default, default);
            Assert.True(result.IsFailure);
            Assert.Equal(expectedMessage, result.Message);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("\r\n")]
        [InlineData("\t")]
        public void When_register_microservice_with_null_or_empty_or_whitespace_name_then_result_is_failure(string microserviceName)
        {
            // Arrange
            var expectedMessage = Messages.MicroserviceNameCanNotBeNullOrEmptyOrWhitespace();
            var serviceNames = new [] {"service"};
            // Act
            var result = _sut.RegisterMicroservice(microserviceName, serviceNames);

            // Assert
            _microservices.DidNotReceiveWithAnyArgs().TryAdd(default, default);
            Assert.True(result.IsFailure);
            Assert.Equal(expectedMessage, result.Message);
        }        
        
        [Fact]
        public void When_register_microservice_that_already_exists_then_result_is_failure()
        {
            // Arrange
            var microserviceName = "microserviceTest";
            var expectedMessage = Messages.MicroserviceHasBeenAlreadyRegisteredMessage(microserviceName);
            var serviceNames = new [] {"service"};
            _microservices.ContainsKey(microserviceName).ReturnsForAnyArgs(true);
            
            // Act
            var result = _sut.RegisterMicroservice(microserviceName, serviceNames);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(expectedMessage, result.Message);
        }
        
        [Fact]
        public void When_register_microservice_with_already_registered_services_then_result_is_failure()
        {
            // Arrange
            var microserviceName = "microserviceTest";
            var serviceName = "service";
            var servicesNames = Enumerable.Repeat(serviceName, 3);
            var expectedMessage = Enumerable
                .Repeat(Messages.ServiceHasBeenAlreadyRegisteredMessage(serviceName), 3)
                .Aggregate(string.Empty, (current, message) => 
                    current + message);
            _microservices.ContainsKey(microserviceName).ReturnsForAnyArgs(false);
            _servicesOwners.ContainsKey(serviceName).Returns(true);
            
            // Act
            var result = _sut.RegisterMicroservice(microserviceName, servicesNames);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(expectedMessage, result.Message);
        }
        
        [Fact]
        public void When_register_valid_microservice_with_valid_services_then_result_is_success()
        {
            // Arrange
            var microserviceName = "microserviceTest";
            var serviceName = "service";
            var servicesNames = Enumerable.Range(0, 3).Select(i => $"serviceName{i}");
            _microservices.ContainsKey(microserviceName).ReturnsForAnyArgs(false);
            _servicesOwners.ContainsKey(serviceName).Returns(false);
            var expectedMessage = Messages.MicroserviceHasBeenRegistered(microserviceName);

            // Act
            var result = _sut.RegisterMicroservice(microserviceName, servicesNames);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedMessage, result.Message);
        }
        
        [Fact]
        public void If_register_instance_with_null_uri_then_result_must_be_failure()
        {
            // Arrange
            var microserviceName = "test";
            var expectedMessage = Messages.CanNotRegisterInstanceWithNullUri(microserviceName);
            
            // Act
            var result = _sut.RegisterInstance(microserviceName, null);
            
            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(expectedMessage, result.Message);
        }
        
        [Fact]
        public void If_register_instance_of_not_existing_microservice_then_result_is_failure_with_proper_message()
        {
            // Arrange
            var microserviceName = "test";
            var uri = new Uri($"{Uri.UriSchemeHttp}://{microserviceName}");
            var expectedMessage = Messages.MicroserviceDoesntExistMessage(microserviceName);
            _instances.ContainsKey(uri).Returns(false);
            
            // Act
            var result = _sut.RegisterInstance(microserviceName, uri);
            
            // Assert
            _microservices.Received().TryGetValue(microserviceName, out _);
            _instances.Received().ContainsKey(uri);
            _logger.Received().Error(expectedMessage);
            Assert.Equal(expectedMessage, result.Message);
            Assert.True(result.IsFailure);
        }

        [Fact]
        public void If_register_already_registered_instance_then_result_is_failure_with_proper_message()
        {
            // Arrange
            var microserviceName = "test";
            var uri = new Uri($"{Uri.UriSchemeHttp}://{microserviceName}");
            var expectedMessage = Messages.InstanceAlreadyRegistered(microserviceName, uri);
            _instances.ContainsKey(uri).Returns(true);

            // Act
            var result = _sut.RegisterInstance(microserviceName, uri);

            // Assert
            _instances.Received().ContainsKey(uri);
            Assert.Equal(expectedMessage, result.Message);
            Assert.True(result.IsFailure);
            Assert.Empty(_instances);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void When_register_instance_for_existing_microservice_then_returned_result_is_the_same_as_in_registration_in_microservice (bool isRegistrationSuccesInMicroservice)
        {
            // Arrange
            var microserviceName = "test";
            var uri = new Uri($"{Uri.UriSchemeHttp}://{microserviceName}");
            var instance = Substitute.For<IMicroserviceInstance>();
            var microservice = Substitute.For<IMicroservice>();
            var registrationResult = Substitute.For<IResult>();
            registrationResult.IsSuccess.Returns(isRegistrationSuccesInMicroservice);
            microservice.RegisterInstance(instance).Returns(registrationResult);
            _instances.ContainsKey(uri).Returns(false);
            _microserviceInstanceFactory
                .Create(microserviceName, uri)
                .Returns(instance);
            _microservices.TryGetValue(microserviceName, out _).Returns(x =>
            {
                x[1] = microservice;
                return true;
            });

            // Act
            var result = _sut.RegisterInstance(microserviceName, uri);

            // Assert
            Assert.Equal(registrationResult, result);
        }

        [Fact]
        public void When_unregister_with_null_then_result_is_failure()
        {
            // Arrange
            var expectedMessage = Messages.CanNotUnregisterInstanceWithNullUri();

            // Act
            var result = _sut.UnregisterInstance(null);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(expectedMessage, result.Message);
        }
        
        [Fact]
        public void When_unregister_not_existing_instance_then_result_is_failure()
        {
            // Arrange
            var uri = new Uri($"{Uri.UriSchemeHttp}://test");
            var expectedMessage = Messages.InstanceDoesntExistMessage(uri);
            _instances.Remove(uri, out _).Returns(false);

            // Act
            var result = _sut.UnregisterInstance(uri);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(expectedMessage, result.Message);
        }
        
        [Fact]
        public void When_microservice_not_exists_then_result_is_failure()
        {
            // Arrange
            var uri = new Uri($"{Uri.UriSchemeHttp}://test");
            const string microserviceName = "Microservice";
            var instance = Substitute.For<IMicroserviceInstance>();
            instance.MicroServiceName.Returns(microserviceName);
            var expectedMessage = Messages.MicroserviceDoesntExistMessage(instance.MicroServiceName);
            _instances.Remove(uri, out _).Returns(x =>
            {
                x[1] = instance;
                return true;
            });
            _microservices.TryGetValue(instance.MicroServiceName, out _).Returns(false);
            
            // Act
            var result = _sut.UnregisterInstance(uri);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(expectedMessage, result.Message);
        }
        
        [Fact]
        public void When_unregistered_instance_with_success()
        {
            // Arrange
            var uri = new Uri($"{Uri.UriSchemeHttp}://test");
            const string microserviceName = "Microservice";
            var instance = new MicroserviceInstance(microserviceName, uri);
            var microservice = Substitute.For<IMicroservice>();
            var expectedMessage = Messages.InstanceSuccessfullyUnregistered(instance);
            _instances.Remove(uri, out _).Returns(x =>
            {
                x[1] = instance;
                return true;
            });
            _microservices.TryGetValue(instance.MicroServiceName, out _).Returns(x =>
            {
                x[1] = microservice;
                return true;
            });
            
            // Act
            var result = _sut.UnregisterInstance(uri);

            // Assert
            microservice.Received().UnregisterInstance(uri);
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedMessage, result.Message);
        }
    }
}