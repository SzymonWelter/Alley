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
}