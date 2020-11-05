using System.IO;
using System.IO.Abstractions;
using Alley.Definitions;
using Alley.Definitions.Factories.Interfaces;
using Alley.Definitions.Interfaces;
using Alley.Definitions.Wrappers.Interfaces;
using Alley.Utils;
using Alley.Utils.Models;
using NSubstitute;
using Serilog;
using Xunit;

namespace Alley.Tests
{
    public class MicroserviceDefinitionBuilderTests
    {

        private readonly MicroserviceDefinitionBuilder _sut;
        private readonly IMicroserviceDescriptor _descriptor;
        private readonly ITextReaderFactory _textReaderFactory;
        private readonly IAlleyLogger _logger;
        private const string ServiceName = "TestService";
        
        public MicroserviceDefinitionBuilderTests()
        {
            _logger = Substitute.For<IAlleyLogger>();
            _descriptor = Substitute.For<IMicroserviceDescriptor>();
            _textReaderFactory = Substitute.For<ITextReaderFactory>();
            _sut = new MicroserviceDefinitionBuilder(_descriptor, _textReaderFactory, _logger);
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
            var fileInfo = MockFileInfo(fileName);

            var errorMessage = "errorMessage";
            var textReaderResult = MockResult<TextReader>(false, errorMessage);

            _textReaderFactory.Create(fileInfo.FullName).Returns(textReaderResult);
            

            // Act
            _sut.AddProto(fileInfo);

            // Assert
            _logger.Received().LogResult(textReaderResult);
            _descriptor.DidNotReceiveWithAnyArgs().Read(default, default);
        }


        [Fact]
        public void WhenProtoFileExistsAndReadingProtoIsFailure_ThenResultIsFailureAndProperErrorMessageIsReturned()
        {
            // Arrange
            var fileName = "test.proto";
            var fileInfo = MockFileInfo(fileName);

            var textReaderMessage = "textReaderMessage";
            var textReader = Substitute.For<TextReader>();
            var textReaderResult = MockResult(true, textReaderMessage, textReader);
            _textReaderFactory.Create(fileInfo.FullName).ReturnsForAnyArgs(textReaderResult);

            var readErrorMessage = "readErrorMessage";
            var readResult = MockResult(false, readErrorMessage);
            _descriptor.Read(fileName, textReaderResult.Value).Returns(readResult);

            // Act
            _sut.AddProto(fileInfo);

            // Assert
            _descriptor.Received().Read(fileName, textReaderResult.Value);
            _logger.Received().LogResult(readResult);

        }
        
        [Fact]
        public void WhenProtoFileExistsAndReadingProtoIsSuccessful_ThenAddingProtoIsSuccessfulAndInformationIsLogged()
        {
            // Arrange
            var fileName = "test.proto";
            var fileInfo = MockFileInfo(fileName);
            
            var textReaderErrorMessage = "textReaderErrorMessage";
            var textReader = Substitute.For<TextReader>();
            var textReaderResult = MockResult(true, textReaderErrorMessage, textReader);
            _textReaderFactory.Create(fileInfo.FullName).ReturnsForAnyArgs(textReaderResult);

            var readMessage = "readMessage";
            var readResult = MockResult(false, readMessage);
            _descriptor.Read(fileName, textReaderResult.Value).Returns(readResult);


            // Act
            _sut.AddProto(fileInfo);

            // Assert
            _descriptor.Received().Read(fileName, textReaderResult.Value);
            _logger.Received().LogResult(readResult);
        }

        private static IResult MockResult(bool isSuccess, string message)
        {
            return MockResult<string>(isSuccess, message);
        }
        
        private static IResult<T> MockResult<T>(bool isSuccess, string message, T textReader = null) where T : class
        {
            var textReaderResult = Substitute.For<IResult<T>>();
            textReaderResult.IsSuccess.Returns(isSuccess);
            textReaderResult.IsNotHandled.Returns(true);
            textReaderResult.Message.Returns(message);
            textReaderResult.Value.Returns(textReader);
            return textReaderResult;
        }

        private static IFileInfo MockFileInfo(string fileName)
        {
            var fullName = Path.GetFullPath(fileName);
            var fileInfo = Substitute.For<IFileInfo>();
            fileInfo.Name.Returns(fileName);
            fileInfo.FullName.Returns(fullName);
            return fileInfo;
        }
    }
}
