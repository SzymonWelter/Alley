using Alley.Definitions.Interfaces;
using Alley.Definitions.Mappers.Interfaces;
using Alley.Definitions.Models.Interfaces;
using Grpc.Core;

namespace Alley.Definitions.Mappers
{
    public class MethodMapper : IMethodMapper
    {
        public Method<IAlleyMessageModel, IAlleyMessageModel> Map(IGrpcMethodDefinition methodDefinition)
        {
            throw new System.NotImplementedException();
        }
    }
}