using System;
using System.Collections.Generic;
using Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Extensions
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
            var assemblyTypesProvider = new SolutionTypesProvider();
            var handlerProvider = new HandlerProvider(assemblyTypesProvider);
            collection.AddAllTransient(handlerProvider.Handlers);

            collection.AddTransient<IMessageBus, MessageBus>();
            collection.AddSingleton<IMessageTypeProvider, MessageTypeProvider>();
            collection.AddSingleton<IMessageProvider, MessageProvider>();
            collection.AddSingleton<IHandlerTypeProvider, HandlerTypeProvider>();
            collection.AddSingleton<IAssemblyTypesProvider, SolutionTypesProvider>();
        }
    }
}
