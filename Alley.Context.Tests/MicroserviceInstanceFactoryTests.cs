using System;
using Alley.Context.Factories;
using Alley.Context.Metrics;
using Xunit;

namespace Alley.Context.Tests
{
    public class MicroserviceInstanceFactoryTests
    {
        [Fact]
        public void Factory_should_create_correct_instance()
        {
            // Arrange
            var sut = new MicroserviceInstanceFactory();
            var microserviceName = "Microservice";
            var uri = new Uri("http://test:5000");
            
            // Act
            var result = sut.Create(microserviceName, uri);

            // Assert
            Assert.Equal(microserviceName, result.MicroServiceName);
            Assert.Equal(uri, result.Uri);
            Assert.Contains(result.Metrics, m =>
                m.Key == MetricType.ActiveConnection &&
                m.Value.Value == 0);

        }
    }
}