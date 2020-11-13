using Alley.Context.Models;
using Alley.Context.Models.Interfaces;
using Xunit;

namespace Alley.Context.Tests
{
    public class MicroserviceTests
    {
        private readonly IMicroservice<IMethodMock> _sut;

        public MicroserviceTests()
        {
            _sut = new Microservice<IMethodMock>();
        }
        
        [Fact]
        public void Test1()
        {
        }
        private interface IMethodMock{}
    }
}