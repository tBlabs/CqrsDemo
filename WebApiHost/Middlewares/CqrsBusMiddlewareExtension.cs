using Microsoft.AspNetCore.Builder;

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