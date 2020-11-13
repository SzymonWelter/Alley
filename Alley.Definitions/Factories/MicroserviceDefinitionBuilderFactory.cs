using Alley.Definitions.Factories.Interfaces;
using Alley.Definitions.Interfaces;
using Alley.Definitions.Wrappers;
using Alley.Utils;
using Google.Protobuf.Reflection;

namespace Alley.Definitions.Factories
{
    public class MicroserviceDefinitionBuilderFactory : IMicroserviceDefinitionBuilderFactory
    {
        public IMicroserviceDefinitionBuilder Create()
        {
            return new MicroserviceDefinitionBuilder(
                new MicroserviceDescriptor(
                    new FileDescriptorSetWrapper(
                        new FileDescriptorSet())), 
                new TextReaderFactory(), 
                new AlleyLogger(Serilog.Log.Logger)
                );
        }
    }
}