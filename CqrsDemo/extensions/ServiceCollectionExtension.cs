using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using tBlabs.Cqrs.Core.Services;

namespace tBlabs.Cqrs.Core.Extensions
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
            var solutionTypesProvider = new SolutionTypesProvider();
            var handlersProvider = new HandlersProvider(solutionTypesProvider);
            collection.AddAllTransient(handlersProvider.Handlers);

			collection.AddSingleton<ITypesProvider, SolutionTypesProvider>();
			collection.AddSingleton<IMessageTypeProvider, MessageTypeProvider>();
            collection.AddSingleton<IMessageProvider, MessageProvider>();
			collection.AddSingleton<IHandlerTypeProvider, HandlerTypeProvider>();
            collection.AddTransient<IMessageBus, MessageBus>();
        }
    }
}
