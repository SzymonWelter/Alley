using System.IO.Abstractions;
using System.Linq;
using Alley.Definitions;
using Alley.Definitions.Interfaces;
using Alley.Definitions.Models.Interfaces;
using Alley.Utils.Configuration;
using NSubstitute;
using Xunit;

namespace Alley.Tests
{
    public class MicroservicesDefinitionsProviderTests
    {
        private readonly MicroservicesDefinitionsProvider _sut;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IMicroserviceDefinitionBuilder _microserviceBuilder;
        private static readonly int DirectoriesCount = 3;
        private static readonly int FilesCount = 5;

        public MicroservicesDefinitionsProviderTests()
        {
            _microserviceBuilder = Substitute.For<IMicroserviceDefinitionBuilder>();
            _configurationProvider = Substitute.For<IConfigurationProvider>();
            _sut = new MicroservicesDefinitionsProvider(_microserviceBuilder, _configurationProvider);
        }

        [Fact]
        public void WhenGetMicroservicesDefinitions_ThenServicesWithNamesEqualsToDirectoriesNameShouldBeReturned()
        {
            // Arrange
            var localizationName = "localizationName";
            var localization = Substitute.For<IDirectoryInfo>();
            var directoriesInfo = MockDirectoriesInfo();
            localization.GetDirectories().Returns(directoriesInfo);
            localization.Name.Returns(localizationName);
            
            _configurationProvider.GetProtosLocalization().Returns(localization);

            _microserviceBuilder.Build(default).ReturnsForAnyArgs(x => MockMicroserviceDefinition((string)x[0]));

                // Act
            var result = _sut.GetMicroservicesDefinitions();

            // Assert
            foreach (var directoryInfo in directoriesInfo)
            {
                Assert.Contains(result, d => d.Name == directoryInfo.Name);
            }
        }

        private IMicroserviceDefinition MockMicroserviceDefinition(string serviceName)
        {
            var mock = Substitute.For<IMicroserviceDefinition>();
            mock.Name.Returns(serviceName);
            return mock;
        }

        private IDirectoryInfo[] MockDirectoriesInfo()
        {
            return Enumerable
                .Range(0, DirectoriesCount)
                .Select(MockDirectoryInfo)
                .ToArray();
        }

        private IDirectoryInfo MockDirectoryInfo(int serviceNumber)
        {
            var directoryInfo = Substitute.For<IDirectoryInfo>();
            directoryInfo.Name.Returns($"Service-{serviceNumber}");

            var filesInfo = Enumerable.Repeat(Substitute.For<IFileInfo>(), FilesCount).ToArray();
            directoryInfo.GetFiles(default).ReturnsForAnyArgs(filesInfo);
            return directoryInfo;
        }
    }
}