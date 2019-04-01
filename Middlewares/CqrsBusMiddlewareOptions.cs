namespace Middlewares
{
	public class CqrsBusMiddlewareOptions
	{
		public string UploadedFilesDir { get; set; }

		public static CqrsBusMiddlewareOptions Default =>
			new CqrsBusMiddlewareOptions() { UploadedFilesDir = "Default dir" };
	}
}