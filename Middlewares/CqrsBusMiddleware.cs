using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Services;
using Microsoft.AspNetCore.Http;
using Middlewares.Extensions;
using Newtonsoft.Json;

namespace Middlewares
{
	public class CqrsBusMiddleware
	{
		private readonly RequestDelegate _nextMiddleware;

		private readonly CqrsBusMiddlewareOptions _options;

		public CqrsBusMiddleware(
			RequestDelegate nextMiddleware,
			CqrsBusMiddlewareOptions options)
		{
			_nextMiddleware = nextMiddleware;
			_options = options;
		}

		public async Task InvokeAsync(HttpContext context, IMessageBus messageBus)
		{
			var requestPath = context.Request.Path;

			if (requestPath == "/File")
			{
				try
				{
					var stream = context.Request.Body;
					//	var form = context.Request.Form;
					var dir = _options.UploadedFilesDir + @"\" + Guid.NewGuid();
					using (var file = File.Create(dir))
					{
						await stream.CopyToAsync(file);
					}

					context.Response.StatusCode = (int)HttpStatusCode.Created;
					await context.Response.WriteAsync(dir);
				}
				catch (Exception e)
				{
					context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
					await context.Response.WriteAsync(e.Message);
				}
			}
			else
			if (requestPath == "/CqrsBus")
			{
				try
				{
					var requestBody = context.Request.Body.ReadAsString();

					var result = await messageBus.ExecuteFromJson(requestBody);

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