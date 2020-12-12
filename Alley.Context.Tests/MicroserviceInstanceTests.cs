using System;
using System.Collections.Generic;
using Alley.Context.Metrics;
using Alley.Context.Models;
using Alley.Context.Models.Interfaces;
using Grpc.Core;
using Xunit;

namespace Alley.Context.Tests
{
    public class MicroserviceInstanceTests
    {
        private const string MicroserviceName = "microservice";
        private readonly Uri _instanceUri;
        private readonly Dictionary<MetricType, IInstanceMetric> _metrics;
        private readonly MicroserviceInstance _sut;

        public MicroserviceInstanceTests()
        {
            _instanceUri = new Uri("http://test:5000/test");
            _metrics = new Dictionary<MetricType, IInstanceMetric>();
            _sut = new MicroserviceInstance(MicroserviceName, _instanceUri, _metrics);
        }

        [Fact]
        public void Constructor_should_assign_properties()
        {
            // Assert
            Assert.Equal(MicroserviceName, _sut.MicroServiceName);
            Assert.Equal(_instanceUri, _sut.Uri);
            Assert.Equal(_metrics, _sut.Metrics);
        }

        [Fact]
        public void Get_Channel_should_return_correct_channel()
        {
            // Arrange
            var expectedTarget = _instanceUri.Authority;
            
            // Act
            var channel = _sut.GetChannel();

            // Assert
            Assert.Equal(expectedTarget, channel.Target);
        }

        [Fact]
        public void Dispose_should_dispose_channel()
        {
            // Arrange
            var channel = _sut.GetChannel();
            _sut.Dispose();

            // Act
            var creatingCallInvoker = new Func<CallInvoker>(() => channel.CreateCallInvoker());

            // Assert
            Assert.Throws<ObjectDisposedException>(creatingCallInvoker);
        }
    }
}