using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;

namespace WebApiHost
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var dependencies = DependencyContext.Default.RuntimeLibraries;

			foreach (var library in dependencies)
			{
				try
				{
					Assembly.Load(new AssemblyName(library.Name));
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
				}
			}

			CreateWebHostBuilder(args).Build().Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();
	}
}
