using System;
using System.IO.Abstractions;
using Alley.Definitions.Factories.Interfaces;
using Alley.Definitions.Interfaces;
using Alley.Definitions.Models;
using Alley.Definitions.Models.Interfaces;
using Alley.Definitions.Wrappers.Interfaces;
using Alley.Utils;
using Alley.Utils.Models;

namespace Alley.Definitions
{
    internal class MicroserviceDefinitionBuilder : IMicroserviceDefinitionBuilder
    {
        private readonly IMicroserviceDescriptor _microserviceDescriptor;
        private readonly ITextReaderFactory _textReaderFactory;
        private readonly IAlleyLogger _logger;

        public MicroserviceDefinitionBuilder(IMicroserviceDescriptor microserviceDescriptor,
            ITextReaderFactory textReaderFactory, IAlleyLogger logger)
        {
            _microserviceDescriptor = microserviceDescriptor;
            _textReaderFactory = textReaderFactory;
            _logger = logger;
        }

        public void AddProto(IFileInfo proto)
        {
            IResult result;

            var textReaderCreationResult = _textReaderFactory.Create(proto.FullName);
            if (textReaderCreationResult.IsSuccess)
            {
                using var protoStream = textReaderCreationResult.Value;
                result = _microserviceDescriptor.Read(proto.Name, protoStream);
            }
            else
            {
                result = textReaderCreationResult;
            }

            _logger.LogResult(result);
        }

        public IMicroserviceDefinition Build(string name)
        {
            var services = _microserviceDescriptor.GetServices();
            return new MicroserviceDefinition(name, services);
        }
    }
}