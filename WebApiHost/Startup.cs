﻿using System.IO;
using Core.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Middlewares;
using Middlewares.Extensions;

namespace WebApiHost
{
	public class Startup
	{
		public Startup(//IConfiguration configuration
				 )
		{
			//	Configuration = configuration;
		}

		//	public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCqrs();
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			//app.UseDeveloperExceptionPage();
			//app.UseHttpsRedirection();
			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles")),
				RequestPath = "/files"
			});
			app.UseCqrsBus(new CqrsBusMiddlewareOptions { UploadedFilesDir = @"d:\files" });
		}
	}
}
