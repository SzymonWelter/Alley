using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Alley.Definitions;
using Alley.Definitions.Interfaces;
using Alley.Definitions.Models.Interfaces;
using Alley.Definitions.Wrappers;
using Alley.Definitions.Wrappers.Interfaces;
using Alley.Utils;
using Google.Protobuf.Reflection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Alley.Tests
{
    public class MicroserviceDescriptorTests
    {
        private readonly MicroserviceDescriptor _sut;
        private readonly IFileDescriptorSet _fileDescriptorSet;
        private const int FilesCount = 5;
        private const int ServicesCount = 5;

        public MicroserviceDescriptorTests()
        {
            _fileDescriptorSet = Substitute.For<IFileDescriptorSet>();
            _sut = new MicroserviceDescriptor(_fileDescriptorSet);
        }

        [Fact]
        public void WhenAddingProtoIsFailed_ThenResultShouldBeFailureAndNotHandledAndContainsErrorMessage()
        {
            // Arrange
            var fileName = "fileName";
            var textReader = Substitute.For<TextReader>();
            var expectedMessage = Messages.FileCanNotBeAddedToDescriptor(fileName);
            _fileDescriptorSet.Add(fileName, true, textReader).Returns(false);
            
            // Act
            var result = _sut.Read(fileName, textReader);
            
            // Assert
            Assert.True(result.IsFailure);
            Assert.True(result.IsNotHandled);
            Assert.Equal(expectedMessage, result.Message);
            _fileDescriptorSet.Received().Add(fileName, true, textReader);
        }
        
        [Fact]
        public void WhenAddingProtoIsSuccess_ThenResultShouldBeSuccessAndNotHandledAndContainsInformationMessage()
        {
            // Arrange
            var fileName = "fileName";
            var textReader = Substitute.For<TextReader>();
            var expectedMessage = Messages.FileHasBeenAddedToDescriptor(fileName);
            _fileDescriptorSet.Add(fileName, true, textReader).Returns(true);
            
            // Act
            var result = _sut.Read(fileName, textReader);
            
            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.IsNotHandled);
            Assert.Equal(expectedMessage, result.Message);
            _fileDescriptorSet.Received().Add(fileName, true, textReader);
        }

        [Fact]
        public void WhenAddingProtoThrownException_ThenResultShouldBeFailureAndNotHandledAndContainsExceptionMessage()
        {
            // Arrange
            var fileName = "fileName";
            var textReader = Substitute.For<TextReader>();
            var exceptionMessage = Messages.FileHasBeenAddedToDescriptor(fileName);
            var exception = new Exception(exceptionMessage);
            _fileDescriptorSet.Add(fileName, true, textReader).Throws(exception);
            
            // Act
            var result = _sut.Read(fileName, textReader);
            
            // Assert
            Assert.True(result.IsFailure);
            Assert.True(result.IsNotHandled);
            Assert.Equal(exceptionMessage, result.Message);
            _fileDescriptorSet.Received().Add(fileName, true, textReader);
        }

        [Fact]
        public void WhenGetServices_ThenFileDescriptorShouldProcessAndReturnsCollectionOfServicesDefinitions()
        {
            // Arrange
            MockFileDescriptorSet();
            var expectedServicesCount = FilesCount * ServicesCount;
            // Act
            var result = _sut.GetServices();

            // Assert
            _fileDescriptorSet.Received().Process();
            Assert.Equal(expectedServicesCount, result.Count());
            AssertServicesNames(result);
        }

        private static void AssertServicesNames(IEnumerable<IGrpcServiceDefinition> result)
        {
            for (var i = 0; i < FilesCount; i++)
            {
                for (var j = 0; j < ServicesCount; j++)
                {
                    Assert.Contains(
                        result,
                        s => s.Name == $"file{i}-service{j}");
                }
            }
        }

        private void MockFileDescriptorSet()
        {            
            var fileList = new List<FileDescriptorProto>();
            for (var i = 0; i < FilesCount; i++)
            {
                var file = CreateFileDescriptor($"file{i}");
                fileList.Add(file);
            }

            _fileDescriptorSet.Files.Returns(fileList);
        }

        private FileDescriptorProto CreateFileDescriptor(string fileName)
        {
            var serviceList = new List<ServiceDescriptorProto>();
            
            for (var i = 0; i < ServicesCount; i++)
            {
                var service = new ServiceDescriptorProto{Name = $"{fileName}-service{i}"};
                serviceList.Add(service);
            }
            
            var fileDescriptor = new FileDescriptorProto();
            fileDescriptor.Services.AddRange(serviceList);
            return fileDescriptor;
        }
    }
}