using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.WebUtilities;
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
			//try
			//{
			//	//context.Request.Body.Position = 0;
			//	var form = await context.Request.ReadFormAsync();

			//}
			//catch (Exception e)
			//{
			//	Console.WriteLine(e);
			//	throw;
			//}

			var requestPath = context.Request.Path;
			//var 

			if (requestPath.Value.StartsWith("/File"))
			{
				try
				{
					//if (context.Request.HasFormContentType)
					{
					//	var form = await context.Request.ReadFormAsync();
					}
					//context.Request.EnableRewind();

					Stream stream = context.Request.Body;
					MemoryStream ms = new MemoryStream();
					stream.CopyTo(ms);
					//var m = ms.ToArray();

					var reader = new MultipartReader(context.Request.GetMultipartBoundary(), ms);
					
					var mps = await reader.ReadNextSectionAsync();
					//mps.Body
					while (mps != null)
					{
						mps = await reader.ReadNextSectionAsync();
					}

					//	var boundary = context.Request.GetMultipartBoundary();
					var dir = _options.UploadedFilesDir + @"\" + Guid.NewGuid();
					using (var file = File.Create(dir))
					{
						await ms.CopyToAsync(file);
					}

					//stream.Seek(0, SeekOrigin.Begin);
					 dir = _options.UploadedFilesDir + @"\" + Guid.NewGuid();
					using (var file = File.Create(dir))
					{
						await ms.CopyToAsync(file);
					}
					 dir = _options.UploadedFilesDir + @"\" + Guid.NewGuid();
					using (var file = File.Create(dir))
					{
						await ms.CopyToAsync(file);
					}

					ms.Dispose();

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