using System.IO;
using Alley.Definitions;
using NSubstitute;
using Xunit;

namespace Alley.Tests
{
    public class MicroserviceDescriptorTests
    {
        private MicroserviceDescriptor _sut;

        public MicroserviceDescriptorTests()
        {
            _sut = new MicroserviceDescriptor();
        }
        
        [Fact]
        public void WhenFileNameIsNull_ThenFailureResultShouldBeReturned()
        {
            // Arrange
            string fileName = null;
            Stream proto = Substitute.ForPartsOf<Stream>();
            
            //Act
            var result = _sut.Read(fileName, proto);
            
            //Assert
            Assert.True(result.IsFailure);
        }
        
        [Fact]
        public void WhenStreamIsNull_ThenFailureResultShouldBeReturned()
        {
            // Arrange
            string fileName = "test";
            Stream proto = null;
            
            //Act
            var result = _sut.Read(fileName, proto);
            
            //Assert
            Assert.True(result.IsFailure);
        }

        [Fact]
        public void WhenStreamAndFileNameIsNull_ThenFailureResultShouldBeReturned()
        {
            // Arrange
            string fileName = null;
            Stream proto = null;
            
            //Act
            var result = _sut.Read(fileName, proto);
            
            //Assert
            Assert.True(result.IsFailure);
        }

    }
}