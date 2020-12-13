using System;
using Microsoft.Extensions.DependencyInjection;

namespace Alley.Utils
{
    public class AlleyServiceProviderFactory : IServiceProviderFactory<IServiceCollection>
    {
        private readonly IServiceCollection _serviceCollection;
        private IServiceProvider _provider;

        public AlleyServiceProviderFactory()
        {
            _serviceCollection = new ServiceCollection();
        }

        public IServiceCollection CreateBuilder(IServiceCollection services)
        {
            var serviceDescriptors = new ServiceDescriptor[services.Count];
            services.CopyTo(serviceDescriptors, 0);

            foreach (var service in serviceDescriptors)
            {
                if (!_serviceCollection.Contains(service))
                {
                    _serviceCollection.Add(service);
                }
            }

            return _serviceCollection;
        }

        public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder)
        {
            CreateBuilder(containerBuilder);
            return CreateServiceProvider();
        }
        public IServiceProvider CreateServiceProvider()
        {
            return _provider ??= _serviceCollection.BuildServiceProvider();
        }
    }
}