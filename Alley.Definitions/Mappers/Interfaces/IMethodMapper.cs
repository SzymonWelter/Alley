using Grpc.Core;
using Alley.Definitions.Models.Interfaces;

namespace Alley.Definitions.Mappers.Interfaces
{
    public interface IMethodMapper
    {
        public Method<IAlleyMessageModel, IAlleyMessageModel> Map(IGrpcMethodDefinition methodDefinition);
    }
}