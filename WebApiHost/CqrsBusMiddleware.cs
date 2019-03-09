using System.Threading.Tasks;
using Core;
using Core.Cqrs;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace WebApiHost
{
	public class CqrsBusMiddleware // dlaczego ta klasa nie dziedziczy po interfejsie?
	{
		private readonly RequestDelegate _next;
		private readonly IMessageBus _bus;

		public CqrsBusMiddleware(RequestDelegate next,
			//IApplicationBuilder app   // Dlaczego tego nie można aktywaować?
			IMessageBus bus)
		{
			_next = next;
			_bus = bus;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			var request = context.Request.Body.ReadAsString();

			var res = _bus.ExecuteFromJson(request);

			var response = new AppResponse<object>();
			response.IsException = false;
			response.Response = res;

			await context.Response.WriteAsync(JsonConvert.SerializeObject(response));

			//	await _next(context); // Z tym nie dziala ale bez tego trace rozszerzalność!
		}
	}
}