using System.IO;

namespace WebApiHost
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