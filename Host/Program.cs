using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using tBlabs.Cqrs.Core.Extensions;

namespace ConsoleAppHost
{
    public class Program
    {
        private static void Main(string[] args)
        {
	        LoadAssembliesToAllowReflectionAccessOtherSolutionProjects();

	        IServiceCollection services = new ServiceCollection();
			services.AddCqrs();
            services.AddTransient<App>();
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            App app = (App) serviceProvider.GetService(typeof(App));

            Task.Run(async () => { await app.Run(); }).Wait();
        }

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

