using Microsoft.Extensions.DependencyInjection;

namespace Core.DI
{
    public interface IDependencyInjectionConfig
    {
        void ConfigureServices(IServiceCollection services);
    }
}