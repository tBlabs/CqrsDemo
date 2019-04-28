using Microsoft.AspNetCore.Builder;

namespace tBlabs.Cqrs.Middleware.Extensions
{
	public static class CqrsBusMiddlewareExtension
	{
		public static IApplicationBuilder UseCqrsBus(this IApplicationBuilder builder, CqrsBusMiddlewareOptions options = null)
		{
			return builder.UseMiddleware<CqrsBusMiddleware>(options ?? CqrsBusMiddlewareOptions.Default);
		}
	}
}