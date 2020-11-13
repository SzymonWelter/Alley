using System;
using System.Threading.Tasks;
using Alley.Context.Providers;
using Alley.Core.Factories;
using Alley.Definitions;
using Alley.Definitions.Factories;
using Alley.Definitions.Factories.Interfaces;
using Alley.Definitions.Interfaces;
using Alley.Definitions.Mappers;
using Alley.Definitions.Mappers.Interfaces;
using Alley.Serialization.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ConfigurationProvider = Alley.Utils.Configuration.ConfigurationProvider;
using IConfigurationProvider = Alley.Utils.Configuration.IConfigurationProvider;

namespace Alley
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            
            var serviceProvider = serviceCollection.BuildServiceProvider();
 
            await serviceProvider.GetService<Startup>().Run();
        }

        private static void ConfigureServices(ServiceCollection serviceCollection)
        {
            var configuration = BuildConfiguration();
            
            serviceCollection.AddSingleton(configuration);
            serviceCollection
                .AddSingleton<
                    IConnectionDataProvider<IAlleyMessageModel, IAlleyMessageModel>,
                    ConnectionDataProvider<IAlleyMessageModel, IAlleyMessageModel>>();
            serviceCollection
                .AddSingleton<
                    IConnectionSessionFactory<IAlleyMessageModel, IAlleyMessageModel>,
                    ConnectionSessionFactory<IAlleyMessageModel, IAlleyMessageModel>>();
            
            serviceCollection.AddTransient<IConfigurationProvider, ConfigurationProvider>();
            serviceCollection.AddTransient<IMicroservicesDefinitionsProvider, MicroservicesDefinitionsProvider>();
            serviceCollection.AddTransient<IMethodFactory<IAlleyMessageModel, IAlleyMessageModel>, MethodFactory>();
            serviceCollection
                .AddTransient<IMicroserviceDefinitionBuilderFactory, MicroserviceDefinitionBuilderFactory>();
            serviceCollection
                .AddTransient<
                    IMethodHandlerFactory<IAlleyMessageModel, IAlleyMessageModel>,
                    MethodHandlerFactory<IAlleyMessageModel, IAlleyMessageModel>>();
            serviceCollection.AddTransient<Startup>();

        }

        private static IConfiguration BuildConfiguration()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", true, true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
    
}
