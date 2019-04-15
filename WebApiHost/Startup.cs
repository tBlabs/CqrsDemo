using System.IO;
using Core.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Middlewares.Extensions;

namespace WebApiHost
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCqrs();
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			//app.UseDeveloperExceptionPage();
			app.UseHttpsRedirection();
			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles")),
				RequestPath = "/files"
			});
			app.UseCqrsBus();
		}
	}
}
