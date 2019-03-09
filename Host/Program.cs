using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Linq;
using System.Reflection;
using Core;
using Core.core.extensions;
using Messages.Commands;
using Microsoft.Extensions.DependencyModel;

namespace Host
{
    public class Program
    {
        private static void Main(string[] args)
        {
	        var dependencies = DependencyContext.Default.RuntimeLibraries;

	        foreach (var library in dependencies)
	        {
			//	if (library.Name != "Core") continue;
				//if (library.Dependencies.Any(x=>x.Name.StartsWith("Core"))) continue;
		        try
		        {
			      //  Console.WriteLine(library.Name);
			        Assembly.Load(new AssemblyName(library.Name));
					
		        }
		        catch (Exception e)
		        {
			       // Console.WriteLine(e);
		        }
	        }

	        var stp = new SolutionTypesProvider();
			foreach (var stpType in stp.Types)
			{
				//if (stpType.Name.Contains("Query"))
				//Console.WriteLine(stpType.FullName);
			}

			//var h = stp.Types.First(x => x.Name.Contains("Handlers"));
			//Console.WriteLine("+++++", h);

		//	Console.ReadKey();
		//	return;
			IServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            services.AddTransient<App>();
			services.AddCqrs();
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            var mp = serviceProvider.GetService(typeof(IMessageTypeProvider)) as MessageTypeProvider;
            Type sc = mp.GetByName("SampleCommand");
            Console.WriteLine(sc.Name);
            //Console.ReadKey();
			serviceProvider.GetService(typeof(App));
		}

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddCqrs();
        }
    }
}

