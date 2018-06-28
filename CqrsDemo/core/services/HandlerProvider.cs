using System;
using System.Reflection;
using System.Collections.Generic;

namespace CqrsDemo
{
    public class HandlerProvider
    {
        public List<Type> Services { get; } = new List<Type>();

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

                    Services.Add(t);
                }
            }
        }
    }
}

