using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Alley.Management
{
    public class ManagementServerBuilder : IManagementServerBuilder
    {
        private readonly IServiceProviderFactory<IServiceCollection> _serviceProviderFactory;

        public ManagementServerBuilder(IServiceProviderFactory<IServiceCollection> serviceProviderFactory)
        {
            _serviceProviderFactory = serviceProviderFactory;
        }

        public IHost Build()
        {
            return CreateHostBuilder().Build();
        }

        private IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .UseServiceProviderFactory(_serviceProviderFactory)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://*:8080");
                    webBuilder.UseStartup<Startup>();
                });
    }

    public class ManagementServerRunner : IManagementServerRunner
    {
        private IHost _host;

        public void SetHost(IHost host)
        {
            _host = host;
        }
        public void Run()
        {
            _host.Run();
        }
    }

    public interface IManagementServerRunner
    {
        void Run();
    }

    public interface IManagementServerBuilder
    {
        IHost Build();
    }
}