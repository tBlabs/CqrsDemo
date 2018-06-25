using System;
using System.Collections.Generic;
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

        public static void AddCQRS(this IServiceCollection collection)
        {
            var handlersProvider = new HandlersProvider();
            collection.AddAllTransient(handlersProvider.Services);

            collection.AddTransient<IMessageBus, MessageBus>();
            collection.AddSingleton<IMessageTypeProvider, MessageTypeProvider>();
            collection.AddSingleton<IMessageProvider, MessageProvider>();
            collection.AddSingleton<IHandlerTypeProvider, HandlerTypeProvider>();
        }
    }
}
