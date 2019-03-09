using Microsoft.Extensions.DependencyInjection;
using Model.config;
using Model.validation.services;

namespace Messages
{
    public class DependencyInjectionConfig : IDependencyInjectionConfig
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IValidator, MyValidator>();
        }
    }
}
