using System.Collections.Generic;
using System.Linq;
using Alley.Context;
using Alley.Management.Mapping;
using Alley.Management.Models;
using Microsoft.AspNetCore.Mvc;

namespace Alley.Management.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MicroservicesController : ControllerBase
    {
        private readonly IReadonlyInstanceContext _context;
        private readonly IClientMapper _mapper;

        public MicroservicesController(IReadonlyInstanceContext context, IClientMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<MicroserviceDTO> Get()
        {
            var instances = _context.GetInstances();
            return instances
                .GroupBy(i => i.MicroServiceName)
                .Select(g => new MicroserviceDTO
                {
                    Name = g.Key,
                    Instances = g.Select(i => _mapper.Map(i))
                });
        }
    }
}