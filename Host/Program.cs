using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using tBlabs.Cqrs.Core.Extensions;
using tBlabs.Cqrs.Core.Interfaces;
using tBlabs.Cqrs.Core.Services;

namespace ConsoleAppHost
{
    public class ConfigModule : IModule
    {
        public void Register(IServiceCollection services)
        {
           // throw new NotImplementedException();
        }

        public void RegisterCqrsStuff(IMessageTypeProvider messageTypeProvider, IHandlerTypeProvider handlerTypeProvider)
        {
           // throw new NotImplementedException();
        }
    }

    public class Program
    {
        private static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();

            services.AddCqrs()
                .AddModule<FeatureX.ConfigModule>();

            services.AddTransient<App>();

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            var modules = serviceProvider.GetServices<IModule>().ToArray();
            var mtp = serviceProvider.GetService<IMessageTypeProvider>();
            var htp = serviceProvider.GetService<IHandlerTypeProvider>();
            foreach (var module in modules)
            {
                module.RegisterCqrsStuff(mtp, htp);
            }

            foreach (var module in modules)
            {
                Console.WriteLine("MODULE "+module);
            }

            foreach (var s in mtp.MessagesList)
            {
                Console.WriteLine("MESSAGE " + s.ToString());
            }

            foreach (var htpHandler in htp.Handlers)
            {
                Console.WriteLine("HANDLER " + htpHandler.ToString());
            }



            App app = (App)serviceProvider.GetService(typeof(App));

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

