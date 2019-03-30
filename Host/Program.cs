using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Core.Extensions;
using Microsoft.Extensions.DependencyModel;

namespace Host
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

