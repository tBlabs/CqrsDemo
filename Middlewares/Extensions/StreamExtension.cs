using System.IO;

namespace tBlabs.Cqrs.Middleware.Extensions
{
	public static class StreamExtension
	{
		public static string ReadAsString(this Stream stream)
		{
			var reader = new StreamReader(stream);

			return reader.ReadToEnd();	
		}
	}
}