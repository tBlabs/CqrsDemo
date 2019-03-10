using System;
using System.Threading.Tasks;
using Core;
using Core.Cqrs;
using Core.Services;
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
			AppResponse response;

			try
			{
				var res = _bus.ExecuteFromJson(request);

				response = new OkResponse(res);
			}
			catch (Exception e)
			{
				response = new ErrResponse(e);
			}

			context.Response.StatusCode = 200;
			await context.Response.WriteAsync(JsonConvert.SerializeObject(response));

			//	await _next(context); // Z tym nie dziala ale bez tego trace rozszerzalność!
		}
	}
}