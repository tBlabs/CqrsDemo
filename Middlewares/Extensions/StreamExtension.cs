using System.IO;
using System.Threading.Tasks;

namespace tBlabs.Cqrs.Middleware.Extensions
{
	public static class StreamExtension
	{
		public static async Task<string> ReadAsString(this Stream stream)
		{
			var reader = new StreamReader(stream);

			return await reader.ReadToEndAsync();	
		}
	}
}