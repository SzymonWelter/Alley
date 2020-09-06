using System;
using System.IO;
using System.Threading.Tasks;
using Alley.Configuration;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Alley
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            var configuration = BuildConfiguration(args);
            var configurationService = new ConfigurationService(configuration);
            var grpcServerBuilder = new GrpcServerBuilder(configurationService);
            var grpcServer = grpcServerBuilder
                .EnableHttp()
                .ConfigureFromProtos(GetProtosLocalization(configuration))
                .Build();

            grpcServer.Start();
            Log.Information("Listening...");
            await grpcServer.ShutdownTask;
        }

        private static DirectoryInfo GetProtosLocalization(IConfiguration configuration)
        {
            var protosRelativePath = configuration["Protos:Localization"];
            var absoluteRelativePath = Path.GetFullPath(protosRelativePath);
            return new DirectoryInfo(absoluteRelativePath);
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
