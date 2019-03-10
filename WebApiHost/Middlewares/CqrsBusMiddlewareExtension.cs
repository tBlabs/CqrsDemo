using Microsoft.AspNetCore.Builder;
using WebApiHost.Middlewares;

namespace WebApiHost
{
	public static class CqrsBusMiddlewareExtension
	{
		public static IApplicationBuilder UseCqrsBus(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<CqrsBusMiddleware>();
		}
	}
}