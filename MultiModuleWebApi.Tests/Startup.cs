using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using tBlabs.Cqrs.Core.Extensions;
using tBlabs.Cqrs.Middleware.Extensions;

namespace MultiModuleWebApi.Tests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCqrs()
                .AddModule<ModuleA.Config>()
                .AddModule<ModuleB.Config>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseCqrsBus();
        }
    }
}
