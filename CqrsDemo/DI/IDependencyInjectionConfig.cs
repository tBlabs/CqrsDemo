using Microsoft.Extensions.DependencyInjection;

namespace Model.config
{
    public interface IDependencyInjectionConfig
    {
        void ConfigureServices(IServiceCollection services);
    }
}