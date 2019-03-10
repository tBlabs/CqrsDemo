using System;
using System.Net;
using System.Threading.Tasks;
using Core.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace WebApiHost.Middlewares
{
	public class CqrsBusMiddleware
	{
		private readonly RequestDelegate _nextMiddleware;
		private readonly IMessageBus _messageBus;

		public CqrsBusMiddleware(
			RequestDelegate nextMiddleware,
			IMessageBus messageBus)
		{
			_nextMiddleware = nextMiddleware;
			_messageBus = messageBus;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			var request = context.Request.Body.ReadAsString();

			try
			{
				var result = await _messageBus.ExecuteFromJson(request);

				context.Response.StatusCode = (int) HttpStatusCode.OK;
				await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
			}
			catch (Exception e)
			{
				context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
				await context.Response.WriteAsync(e.Message);
			}
		}
	}
}