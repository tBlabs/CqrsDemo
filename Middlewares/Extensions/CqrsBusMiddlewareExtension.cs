using Microsoft.AspNetCore.Builder;

namespace Middlewares.Extensions
{
	public static class CqrsBusMiddlewareExtension
	{
		public static IApplicationBuilder UseCqrsBus(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<CqrsBusMiddleware>();
		}
	}
}