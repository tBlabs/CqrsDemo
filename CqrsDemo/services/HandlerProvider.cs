using System;
using System.Collections.Generic;
using Core.Cqrs;

namespace Core.Services
{
    public class HandlerProvider
    {
        public List<Type> Handlers { get; } = new List<Type>();

        public HandlerProvider(IAssemblyTypesProvider thisAssemblyTypes)
        {
            foreach (var t in thisAssemblyTypes.Types)
            {
                if (!t.IsClass || !t.IsPublic || t.IsAbstract) continue;

                var interfaces = t.GetInterfaces();

                foreach (var i in interfaces)
                {
                    if (!i.IsGenericType) continue;

                    if ((i.GetGenericTypeDefinition() != typeof(ICommandHandler<>))
                        && (i.GetGenericTypeDefinition() != typeof(IQueryHandler<,>))) continue;

                    Handlers.Add(t);
                }
            }
        }
    }
}

