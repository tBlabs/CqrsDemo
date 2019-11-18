using System;

namespace tBlabs.Cqrs.Middleware
{
	public class CqrsBusMiddlewareOptions : ICloneable
	{
		public string EndpointUrl { get; set; } = "/CqrsBus";
		public string MessageHeader { get; set; } = "Message";
        public bool AddStackTrace { get; set; } = false;

		public static CqrsBusMiddlewareOptions Default { get; } = new CqrsBusMiddlewareOptions()
		{
			EndpointUrl = "/CqrsBus",
			MessageHeader = "Message",
            AddStackTrace = false
		};

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}