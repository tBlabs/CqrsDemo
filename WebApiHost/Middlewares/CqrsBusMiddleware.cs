using System;
using System.Net;
using System.Threading.Tasks;
using Core.Exceptions;
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
			var requestBody = context.Request.Body.ReadAsString();
			var requestPath = context.Request.Path;

			if (requestPath == "/CqrsBus")
			{
				try
				{
					var result = await _messageBus.ExecuteFromJson(requestBody);

					context.Response.StatusCode = (int)HttpStatusCode.OK;
					await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
				}
				catch (MessageNotFoundException e)
				{
					context.Response.StatusCode = (int)HttpStatusCode.NotFound;
					await context.Response.WriteAsync(e.Message);
				}
				catch (Exception e)
				{
					context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
					await context.Response.WriteAsync(e.Message);
				}
			}
			else
			{
				await _nextMiddleware(context);
			}
		}
	}
}