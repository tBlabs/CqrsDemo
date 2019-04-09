using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyModel;

namespace WebApiHost
{
	
	//public class TestQuery : IQuery<int>
	//{
	//	public int Value { get; set; }
	//}

	//public class TestQueryHandler : IQueryHandler<TestQuery, Task<int>>
	//{
	//	public Task<int> Handle(TestQuery query)
	//	{
	//		return Task.FromResult(query.Value * 2);
	//	}
	//}

	//public class FileCommand : ICommandWithStream
	//{
	//	public Stream Stream { get; set; }
	//	public string SaveAs { get; set; }
	//}

	//public class FileCommandHandler : ICommandHandler<FileCommand>
	//{
	//	public async Task Handle(FileCommand command)
	//	{
	//		var dir = Directory.GetCurrentDirectory() + @"\" + command.SaveAs;
	//		using (var file = File.Create(dir))
	//		{
	//			await command.Stream.CopyToAsync(file);
	//		}
	//	}
	//}


	public class Program
	{
		public static void Main(string[] args)
		{
			LoadAssembliesToAllowReflectionAccessOtherSolutionProjects();

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
