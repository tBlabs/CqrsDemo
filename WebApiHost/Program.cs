using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyModel;

namespace WebApiHost
{
	public class Program
	{
		public static void Main(string[] args)
		{
		//	LoadAssembliesToAllowReflectionAccessOtherSolutionProjects();

			CreateWebHostBuilder(args).Build().Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();

		private static void LoadAssembliesToAllowReflectionAccessOtherSolutionProjects()
		{       
			var dependencies = DependencyContext.Default.RuntimeLibraries;

			foreach (var library in dependencies)
			{
				try
				{
                    File.AppendAllText("loading.txt", new AssemblyName(library.Name) + Environment.NewLine);
					Assembly.Load(new AssemblyName(library.Name));
				}
				catch (Exception)
				{
					/* do nothing with that exception */
				}
			}
		}
	}
}
