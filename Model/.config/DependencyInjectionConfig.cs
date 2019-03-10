using Core.DI;
using Messages.validation.services;
using Microsoft.Extensions.DependencyInjection;

namespace Messages.config
{
    public class DependencyInjectionConfig : IDependencyInjectionConfig
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IValidator, MyValidator>();
        }
    }
}
