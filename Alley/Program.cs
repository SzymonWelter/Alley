using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Alley
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
        }

        private static IConfiguration BuildConfiguration(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", true, true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
        }
    }
}
