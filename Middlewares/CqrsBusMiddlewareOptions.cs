namespace Middlewares
{
	public class CqrsBusMiddlewareOptions
	{
		public string EndpointUrl { get; set; }

		public static CqrsBusMiddlewareOptions Default =>
			new CqrsBusMiddlewareOptions() { EndpointUrl = "/CqrsBus" };
	}
}