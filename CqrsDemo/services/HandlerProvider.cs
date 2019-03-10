using System;
using System.Collections.Generic;
using Core.Cqrs;

namespace Core.Services
{
    public class HandlerProvider
    {
        public List<Type> Handlers { get; } = new List<Type>();

        public HandlerProvider(ISolutionTypesProvider thisSolutionTypes)
        {
            foreach (var t in thisSolutionTypes.Types)
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

