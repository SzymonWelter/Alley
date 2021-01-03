using Alley.Context.Models.Interfaces;
using Alley.Management.Models;

namespace Alley.Management.Mapping
{
    public interface IClientMapper
    {
        MicroserviceInstanceDTO Map(IReadonlyMicroserviceInstance instance);
    }
}