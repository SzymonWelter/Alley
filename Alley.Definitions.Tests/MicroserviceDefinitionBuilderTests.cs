using System.IO;
using System.IO.Abstractions;
using Alley.Definitions;
using Alley.Utils;
using NSubstitute;
using Serilog;
using Xunit;

namespace Alley.Tests
{
    public class MicroserviceDefinitionBuilderTests
    {

        private readonly MicroserviceDefinitionBuilder _sut;
        private readonly IMicroserviceDescriptor _descriptor;
        private readonly IAlleyLogger _logger;
        private const string ServiceName = "TestService";
        
        public MicroserviceDefinitionBuilderTests()
        {
            _logger = Substitute.For<IAlleyLogger>();
            _descriptor = Substitute.For<IMicroserviceDescriptor>();
            _sut = new MicroserviceDefinitionBuilder(_descriptor, _logger);
        }
        
        [Fact]
        public void WhenNameIsProvided_ThenBuilderReturnsDefinitionWithSpecifiedName()
        {
            // Act
            var result = _sut.Build(ServiceName);
            
            // Assert
            Assert.Equal(ServiceName, result.Name);
        }

        [Fact]
        public void WhenAnyProtoIsNotAdded_ThenBuilderReturnsMicroserviceDefinitionWithEmptyGrpcServiceCollection()
        {
            // Act
            var result = _sut.Build(ServiceName);
            
            // Assert
            Assert.Empty(result.Services);
            _descriptor.Received().GetServices();
        }

        [Fact]
        public void WhenProtoFileNotExists_ThenAddingProtoIsFailureAndProperlyMessageIsReturned()
        {
            // Arrange
            var fileName = "fail.proto";
            var fullName = Path.GetFullPath(fileName);
            var fileInfo = Substitute.For<IFileInfo>();
            fileInfo.Name.Returns(fileName);

            // Act
            _sut.AddProto(fileInfo);

            // Assert
            _logger.ReceivedWithAnyArgs().Error(default);
            _descriptor.DidNotReceiveWithAnyArgs().Read(default, default);
        }

        [Fact]
        public void WhenProtoFileExistsAndReadingProtoIsFailure_ThenResultIsFailureAndProperErrorMessageIsReturned()
        {
            // Arrange
            var fileName = "test.proto";
            var fileInfo = Substitute.For<IFileInfo>();
            fileInfo.Name.Returns(fileName);
            
            var errorMessage = "errorMessage";
            _descriptor.Read(default, default).ReturnsForAnyArgs(Result.Failure(errorMessage));

            // Act
            _sut.AddProto(fileInfo);

            // Assert
            _descriptor.ReceivedWithAnyArgs().Read(default, default);
            _logger.ReceivedWithAnyArgs().Error(default);

        }
        
        [Fact]
        public void WhenProtoFileExistsAndReadingProtoIsSuccessful_ThenAddingProtoIsSuccessfulAndInformationIsLogged()
        {
            // Arrange
            var fileName = "test.proto";
            var fileInfo = Substitute.For<IFileInfo>();
            fileInfo.Name.Returns(fileName);
            
            _descriptor.Read(default, default).ReturnsForAnyArgs(Result.Success());

            // Act
            _sut.AddProto(fileInfo);

            // Assert
            _descriptor.ReceivedWithAnyArgs().Read(default, default);
            _logger.ReceivedWithAnyArgs().Information(default);
        }
        
    }
}