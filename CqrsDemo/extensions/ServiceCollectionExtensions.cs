using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using tBlabs.Cqrs.Core.Interfaces;
using tBlabs.Cqrs.Core.Services;

namespace tBlabs.Cqrs.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAllTransient(this IServiceCollection collection, IEnumerable<Type> services)
        {
            foreach (Type t in services)
            {
                collection.AddTransient(t);
            }
        }

        public static IServiceCollection AddCqrs(this IServiceCollection services)
        {
            // var mtp = new MessageTypeProvider();
            //var htp = new HandlerTypeProvider();

            //            services.AddSingleton<IMessageTypeProvider>(mtp);
            //          services.AddSingleton<IHandlerTypeProvider>(htp);


            //   var solutionTypesProvider = new SolutionTypesProvider();
            //    var handlersProvider = new HandlersProvider(solutionTypesProvider);
            //   collection.AddAllTransient(handlersProvider.Handlers);

            //	collection.AddSingleton<ITypesProvider, SolutionTypesProvider>();
            services.AddSingleton<IMessageProvider, MessageProvider>();
            services.AddSingleton<IMessageTypeProvider, MessageTypeProvider>();
            services.AddSingleton<IHandlerTypeProvider, HandlerTypeProvider>();
            services.AddTransient<IMessageBus, MessageBus>();
            return services;
        }

        public static void AddHandlers<T>(this IServiceCollection services)
        {
            //  var mtp = collection.get
            Type cm = typeof(T);
            var m = cm.Assembly.GetTypes();
            foreach (var type in m)
            {
                var inter = type.GetInterfaces();
                foreach (var type1 in inter)
                {
                    if (type1.IsAssignableFrom(typeof(IModule)))
                    {
                        //Console.WriteLine("is IModule");
                        Object o = Activator.CreateInstance(type);
                        var mod = (IModule)o;
                     //   mod.Register(mtp, htp);
                     //   services.AddAllTransient(htp.Handlers);
                        mod.Register(services);
                    }
                }
            }
        }

        public static IServiceCollection AddModule<T>(this IServiceCollection services) where T: class, IModule
        {
            services.AddHandlers<T>();
            services.AddTransient<IModule, T>();

            return services;
        }
    }
}
