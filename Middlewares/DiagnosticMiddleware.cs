using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using tBlabs.Cqrs.Core.Services;

namespace tBlabs.Cqrs.Middleware
{
    public class DiagnosticMiddleware
    {
        private readonly RequestDelegate _nextMiddleware;

        public DiagnosticMiddleware(RequestDelegate nextMiddleware)
        {
            _nextMiddleware = nextMiddleware;
        }

        public async Task InvokeAsync(HttpContext httpContext, IMessageBus messageBus, IMessageTypeProvider messageTypeProvider)
        {
            var requestPath = httpContext.Request.Path;

            if (requestPath == "/CqrsInfo")
            {
                var s = "Available Messages:" + Environment.NewLine;
                foreach (var s1 in messageTypeProvider.MessagesList)
                {
                    s += "- " + s1 + Environment.NewLine;
                }

                await httpContext.Response.WriteAsync(s);
            }
            else
            {
                await _nextMiddleware(httpContext);
            }
        }
    }
}