using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace CqrsDemo.core.extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddAllTransient(this IServiceCollection collection, IEnumerable<Type> services)
        {
            foreach (Type t in services)
            {
                collection.AddTransient(t);
            }
        }

        public static void AddCqrs(this IServiceCollection collection)
        {
            var assemblyTypesProvider = new AssemblyTypesProvider();
            var handlersProvider = new HandlerProvider(assemblyTypesProvider);
            collection.AddAllTransient(handlersProvider.Services);

            collection.AddTransient<IMessageBus, MessageBus>();
            collection.AddSingleton<IMessageTypeProvider, MessageTypeProvider>();
            collection.AddSingleton<IMessageProvider, MessageProvider>();
            collection.AddSingleton<IHandlerTypeProvider, HandlerTypeProvider>();
            collection.AddSingleton<IAssemblyTypesProvider, AssemblyTypesProvider>();
        }
    }
}
