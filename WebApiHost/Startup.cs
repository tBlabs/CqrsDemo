﻿using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using tBlabs.Cqrs.Core.Extensions;
using tBlabs.Cqrs.Middleware;
using tBlabs.Cqrs.Middleware.Extensions;

namespace WebApiHost
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
        {
            services.AddCqrs();

            services.AddModule<Config>()
                .AddModule<ModuleA.Config>()
                .AddModule<ModuleB.Config>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			app.UseHttpsRedirection();
			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles")),
				RequestPath = "/files"
			});
            app.UseMiddleware<DiagnosticMiddleware>();
			app.UseCqrsBus(new CqrsBusMiddlewareOptions { AddStackTrace = true });
		}
	}
}
