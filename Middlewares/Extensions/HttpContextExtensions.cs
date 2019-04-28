using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace tBlabs.Cqrs.Middleware.Extensions
{
	public static class HttpContextExtensions
	{
		public static async Task NotFound(this HttpResponse response, string message)
		{
			response.StatusCode = (int)HttpStatusCode.NotFound;
			await response.WriteAsync(message);
		}

		public static async Task InternalServerError(this HttpResponse response, string message)
		{
			response.StatusCode = (int)HttpStatusCode.InternalServerError;
			await response.WriteAsync(message);
		}
			
		public static async Task Json(this HttpResponse response, object obj)
		{
			response.StatusCode = (int)HttpStatusCode.OK;
			var serializedObj = JsonConvert.SerializeObject(obj);
			await response.WriteAsync(serializedObj);
		}
	}
}