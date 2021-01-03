using System.Collections.Generic;

namespace Alley.Management.Models
{
    public class MicroserviceDTO
    {
        public string Name{ get; set; }
        public IEnumerable<MicroserviceInstanceDTO> Instances { get; set; }
    }
}