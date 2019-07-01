using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using tBlabs.Cqrs.Core.Interfaces;
using tBlabs.Cqrs.Core.Services;

namespace tBlabs.Cqrs.Middleware.Extensions
{
	public static class CqrsBusMiddlewareExtension
    {
        public static bool isConfigured;

		public static IApplicationBuilder UseCqrsBus(this IApplicationBuilder builder, 
            CqrsBusMiddlewareOptions options = null)
		{
            if (isConfigured == false)
            {
                var serviceProvider = builder.ApplicationServices;

                var modules = serviceProvider.GetServices<IModule>().ToArray();
                var mtp = serviceProvider.GetService<IMessageTypeProvider>();
                var htp = serviceProvider.GetService<IHandlerTypeProvider>();
                foreach (var module in modules)
                {
                    module.RegisterCqrsStuff(mtp, htp);
                }

                isConfigured = true;
            }

			return builder.UseMiddleware<CqrsBusMiddleware>(options ?? CqrsBusMiddlewareOptions.Default);
		}
	}
}