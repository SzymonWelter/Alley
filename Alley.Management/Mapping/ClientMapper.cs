using Alley.Context.Models.Interfaces;
using Alley.Management.Models;

namespace Alley.Management.Mapping
{
    class ClientMapper : IClientMapper
    {
        public MicroserviceInstanceDTO Map(IReadonlyMicroserviceInstance instance)
        {
            return new MicroserviceInstanceDTO
            {
                Uri = instance.Uri
            };
        }
    }
}