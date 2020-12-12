using System;
using System.Threading.Tasks;
using Alley.Context;
using Alley.Context.Factories;
using Alley.Context.LoadBalancing;
using Alley.Core;
using Alley.Core.Handling;
using Alley.Definitions;
using Alley.Definitions.Factories;
using Alley.Definitions.Factories.Interfaces;
using Alley.Definitions.Interfaces;
using Alley.Definitions.Mappers;
using Alley.Definitions.Mappers.Interfaces;
using Alley.Definitions.Wrappers;
using Alley.Definitions.Wrappers.Interfaces;
using Alley.Serialization.Models;
using Alley.Utils;
using Google.Protobuf.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
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

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            var configuration = BuildConfiguration();
            serviceCollection.AddSingleton(configuration);
            
            var logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            serviceCollection.AddSingleton<ILogger>(logger);
            serviceCollection
                .AddSingleton<
                    ISessionFactory<IAlleyMessageModel, IAlleyMessageModel>,
                    SessionFactory<IAlleyMessageModel, IAlleyMessageModel>>();
            serviceCollection
                .AddSingleton<
                    ISessionFactory<IAlleyMessageModel, IAlleyMessageModel>,
                    SessionFactory<IAlleyMessageModel, IAlleyMessageModel>>();
            
            serviceCollection.AddSingleton<IConnectionTargetProvider, LoadBalancingManager>();
            serviceCollection.AddSingleton<ILoadBalancingStrategy, ConnectionCountStrategy>();

            serviceCollection.AddSingleton<MicroserviceContext>();
            serviceCollection.AddSingleton<IMicroserviceContext>(x => x.GetRequiredService<MicroserviceContext>());
            serviceCollection.AddSingleton<IReadonlyInstanceContext>(x => x.GetRequiredService<MicroserviceContext>());
            serviceCollection.AddSingleton<IMetricRepository>(x => x.GetRequiredService<MicroserviceContext>());
            serviceCollection.AddSingleton<IChannelProvider>(x => x.GetRequiredService<MicroserviceContext>());
            
            serviceCollection.AddTransient<
                IGrpcServerBuilder<IAlleyMessageModel, IAlleyMessageModel>, 
                GrpcServerBuilder<IAlleyMessageModel, IAlleyMessageModel>>();
            serviceCollection.AddTransient<IConfigurationProvider, ConfigurationProvider>();
            serviceCollection.AddTransient<IAlleyLogger, AlleyLogger>();
            serviceCollection.AddTransient<IMicroservicesDefinitionsProvider, MicroservicesDefinitionsProvider>();
            serviceCollection.AddTransient<IMicroserviceDefinitionBuilder, MicroserviceDefinitionBuilder>();
            serviceCollection.AddTransient<IMicroserviceDescriptor, MicroserviceDescriptor>();
            serviceCollection.AddTransient<IFileDescriptorSet, FileDescriptorSetWrapper>();
            serviceCollection.AddTransient<FileDescriptorSet>();
            serviceCollection.AddTransient<ITextReaderFactory, TextReaderFactory>();
            serviceCollection.AddTransient<IMethodFactory<IAlleyMessageModel, IAlleyMessageModel>, MethodFactory>();
            serviceCollection.AddTransient<IMicroserviceInstanceFactory, MicroserviceInstanceFactory>();
            serviceCollection
                .AddTransient<IMicroserviceDefinitionBuilderFactory, MicroserviceDefinitionBuilderFactory>();
            serviceCollection
                .AddTransient<
                    IMethodHandlerProvider<IAlleyMessageModel, IAlleyMessageModel>,
                    MethodHandlerProvider<IAlleyMessageModel, IAlleyMessageModel>>();
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