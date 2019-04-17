namespace Middlewares
{
	public class CqrsBusMiddlewareOptions
	{
		public string EndpointUrl { get; set; } = "/CqrsBus";
		public string MessageHeader { get; set; } = "Message";

		public static CqrsBusMiddlewareOptions Default { get; } = new CqrsBusMiddlewareOptions()
		{
			EndpointUrl = "/CqrsBus",
			MessageHeader = "Message"
		};
	}
}