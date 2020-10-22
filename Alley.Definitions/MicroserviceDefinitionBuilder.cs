using System;
using System.IO;
using System.IO.Abstractions;
using Alley.Utils;

namespace Alley.Definitions
{
    internal class MicroserviceDefinitionBuilder : IMicroserviceDefinitionBuilder
    {
        private readonly IMicroserviceDescriptor _microserviceDescriptor;
        private readonly IAlleyLogger _logger;
        public MicroserviceDefinitionBuilder(IMicroserviceDescriptor microserviceDescriptor, IAlleyLogger logger)
        {
            _microserviceDescriptor = microserviceDescriptor;
            _logger = logger;
        }

        public void AddProto(IFileInfo proto)
        {
            IResult result;
            try
            {
                using var protoStream = File.OpenRead(proto.FullName);
                result = _microserviceDescriptor.Read(proto.Name, protoStream);
            }
            catch (Exception e)
            {
                result = Result.Failure(e.Message);
            }
            
            LogResult(result);
        }

        private void LogResult(IResult result)
        {
            if (result.IsSuccess && result.IsNotHandled)
            {
                _logger.Information(result.Message);
            }
            else if (result.IsFailure && result.IsNotHandled)
            {
                _logger.Error(result.Message);
            }
        }

        public IMicroserviceDefinition Build(string name)
        {
            var services = _microserviceDescriptor.GetServices();
            return new MicroserviceDefinition(name, services);
        }
    }
}