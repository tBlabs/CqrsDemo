using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using tBlabs.Cqrs.Core.Exceptions;
using tBlabs.Cqrs.Core.Extensions;
using tBlabs.Cqrs.Core.Services;
using tBlabs.Cqrs.Middleware.Extensions;

namespace tBlabs.Cqrs.Middleware
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

		public async Task InvokeAsync(HttpContext httpContext, IMessageBus messageBus)
		{
			var requestPath = httpContext.Request.Path;

			if (requestPath == _options.EndpointUrl)
			{
				try
				{
					if (IsMessageWithStream(httpContext))
					{
						await ProcessMessageWithStream(httpContext, messageBus);
					}
					else
					{
						await ProcessMessage(httpContext, messageBus);
					}
				}
				catch (NotFoundException e)
				{
					await httpContext.Response.NotFound(e.Message);
				}
				catch (Exception e)
				{
					await httpContext.Response.InternalServerError(e.Message);
				}
			}
			else
			{
				await _nextMiddleware(httpContext);
			}
		}

		private bool IsMessageWithStream(HttpContext context)
		{
			return context.Request.Headers[CqrsBusMiddlewareOptions.Default.MessageHeader].ToString().IsNotEmpty();
		}

		private async Task ProcessMessage(HttpContext context, IMessageBus messageBus)
		{
			string message = context.Request.Body.ReadAsString();

			var messageExecutionResult = await messageBus.Execute(message);

			await context.Response.Json(messageExecutionResult);
		}

		private async Task ProcessMessageWithStream(HttpContext context, IMessageBus messageBus)
		{
			Stream stream = context.Request.Body;
			string message = context.Request.Headers[CqrsBusMiddlewareOptions.Default.MessageHeader];

			var messageExecutionResult = await messageBus.Execute(message, stream);

			await context.Response.Json(messageExecutionResult);
		}
	}
}