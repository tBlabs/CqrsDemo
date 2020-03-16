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
                    if (_options.AddStackTrace)
                        await httpContext.Response.NotFound(e);
                    else
                        await httpContext.Response.NotFound(e.Message);
                }
                catch (Exception e)
                {
                    if (_options.AddStackTrace)
                        await httpContext.Response.InternalServerError(e);
                    else
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
            string message = await context.Request.Body.ReadAsString();

            var messageExecutionResult = await messageBus.Execute(message);

            if (messageExecutionResult is Stream responseStream)
            {
                responseStream.Position = 0; // Łatwo zapomnieć o resecie strumienia, ta operacja zwalnia nas z tego obowiązku ale nie pozwala na przesyłanie strumienia np od połowy...
                await responseStream.CopyToAsync(context.Response.Body);

                await responseStream.DisposeAsync(); // Dispołsować powinien ten kto stworzył ale nie wiem jak to inaczej rozwiązać, może wsadzić messageBus.Execute w usinga?
            }
            else
            {
                await context.Response.Json(messageExecutionResult);
            }
        }

        private async Task ProcessMessageWithStream(HttpContext context, IMessageBus messageBus)
        {
            Stream stream = context.Request.Body;
            string message = context.Request.Headers[CqrsBusMiddlewareOptions.Default.MessageHeader];

            var messageExecutionResult = await messageBus.Execute(message, stream);

            if (messageExecutionResult is Stream responseStream)
            {
                await responseStream.CopyToAsync(context.Response.Body);

                await responseStream.DisposeAsync(); // Dispołsować powinien ten kto stworzył ale nie wiem jak to inaczej rozwiązać, może wsadzić messageBus.Execute w usinga?
            }
            else
            {
                await context.Response.Json(messageExecutionResult);
            }
        }
    }
}