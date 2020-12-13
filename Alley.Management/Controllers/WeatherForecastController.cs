using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alley.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Alley.Management.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IReadonlyInstanceContext _context;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IReadonlyInstanceContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = string.Join('\n', _context.GetInstances("count.Counter").Value.Select( i => i.ToString()))
                })
                .ToArray();
        }
    }
}