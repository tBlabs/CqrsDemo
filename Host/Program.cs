using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using Core.core.extensions;

namespace Host
{
    partial class Program
    {
        private static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            services.AddTransient<App>();
            IServiceProvider serviceProvider = services.BuildServiceProvider();      
   
            serviceProvider.GetService(typeof(App));
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddCqrs();
        }
    }
}

