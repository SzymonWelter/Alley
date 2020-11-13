using Alley.Context.Models.Interfaces;
using Alley.Utils.Models;

namespace Alley.Context.Models
{
    public class Microservice<TMethod> : IMicroservice<TMethod>
    {
        public Result RegisterInstance(IMicroserviceInstance microServiceInstance)
        {
            throw new System.NotImplementedException();
        }

        public Result<TMethod> GetMethod(string methodName)
        {
            throw new System.NotImplementedException();
        }
    }
}