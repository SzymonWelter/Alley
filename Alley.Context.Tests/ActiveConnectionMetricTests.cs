using System;
using Alley.Context.Metrics;
using NSubstitute;
using Xunit;

namespace Alley.Context.Tests
{
    public class ActiveConnectionMetricTests
    {
        private ActiveConnectionMetric _sut;
        private const double ActiveConnections = 1;

        public ActiveConnectionMetricTests()
        {
            _sut = new ActiveConnectionMetric(ActiveConnections);
        }


        [Fact]
        public void Constructor_should_assign_props()
        {
            // Assert
            Assert.Equal(MetricType.ActiveConnection, _sut.Type);
            Assert.Equal(ActiveConnections, _sut.Value);
        }
        
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void Compare_to_should_return_positive_value_if_less_connection_than_another(double anotherActiveConnections)
        {
            // Arrange
            var anotherMetric = new ActiveConnectionMetric(anotherActiveConnections);
            var expectedResult = GetExpectedResult(anotherActiveConnections);
            
            // Act
            var result = _sut.CompareTo(anotherMetric);
            
            // Assert
            Assert.Equal(expectedResult , result);
        }

        [Fact]
        public void When_compare_another_metric_with_different_type_then_correct_exception_thrown()
        {
            // Arrange
            var anotherMetric = Substitute.For<IInstanceMetric>();
            anotherMetric.Type.Returns(MetricType.ProcessorUsage);
            
            // Act
            var comparison = new Action(() => _sut.CompareTo(anotherMetric));

            // Assert
            Assert.Throws<ArgumentException>(comparison);
        }

        private int GetExpectedResult(double anotherActiveConnections)
        {
            return _sut.Value == anotherActiveConnections ? 0 : 
                _sut.Value < anotherActiveConnections ? 1 : -1;
        }
    }
}