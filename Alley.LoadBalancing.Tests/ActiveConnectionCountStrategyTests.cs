using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Alley.Context.Metrics;
using Alley.Context.Models;
using Alley.Context.Models.Interfaces;
using Alley.LoadBalancing.Strategies;
using Alley.Utils;
using Xunit;

namespace Alley.LoadBalancing.Tests
{
    public class ActiveConnectionCountStrategyTests
    {
        private readonly ActiveConnectionCountStrategy _sut;

        public ActiveConnectionCountStrategyTests()
        {
            _sut = new ActiveConnectionCountStrategy();
        }

        [Fact]
        public void If_execute_with_null_then_result_is_failure()
        {
            // Arrange
            var expectedMessage = Messages.CanNotExecuteLoadBalancingStrategyForZeroAvailableInstances();

            // Act
            var result = _sut.Execute(null);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(expectedMessage, result.Message);
        }

        [Fact]
        public void If_execute_with_collection_with_nulls_then_return_failure_result()
        {
            // Arrange
            var instances = new List<IReadonlyMicroserviceInstance> {null, null};
            var expectedMessage = Messages.CanNotFindSuitableTarget(instances);

            // Act
            var result = _sut.Execute(instances);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(expectedMessage, result.Message);
        }

        [Theory]
        [InlineData(new[] {0, 1, 2, 3, 4, 5}, new[] {0})]
        [InlineData(new[] {6, 24, 8, 2, 4, 5}, new[] {3})]
        [InlineData(new[] {0, 0, 0, 2, 0, 5}, new[] {0, 1, 2, 4})]
        [InlineData(new[] {1, 1, 1, 1, 1, 1}, new[] {0, 1, 2, 3, 4, 5})]
        [InlineData(new[] {0}, new[] {0})]
        public void If_there_is_an_instances_with_the_least_active_connection_count_then_correct_uri_should_be_thrown(
            int[] activeConnections, int[] winnerIndexes)
        {
            // Arrange
            var instances = GetInstances(activeConnections);
            var expectedUris = GetCorrectUris(instances, winnerIndexes);

            // Act
            var result = _sut.Execute(instances);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Contains(result.Value, expectedUris);
        }

        private IEnumerable<Uri> GetCorrectUris(IEnumerable<IReadonlyMicroserviceInstance> instances,
            int[] winnerIndexes)
        {
            return winnerIndexes.Select(winnerIndex => instances.ElementAt(winnerIndex).Uri);
        }

        private static IEnumerable<IReadonlyMicroserviceInstance> GetInstances(IEnumerable<int> connectionsCount)
        {
            var i = 0;
            foreach (var connection in connectionsCount)
            {
                yield return new MicroserviceInstance(
                    FormatMicroserviceName(i),
                    GetInstanceUri(i),
                    new Dictionary<MetricType, IInstanceMetric>
                    {
                        {
                            MetricType.ActiveConnection, new ActiveConnectionMetric(connection)
                        }
                    });
                i++;
            }
        }

        private static string FormatMicroserviceName(int index) => $"test{index}";
        private static Uri GetInstanceUri(int index) => new Uri($"http://{FormatMicroserviceName(index)}/");
    }
}