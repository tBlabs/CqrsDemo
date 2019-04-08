﻿using System;
using System.Collections.Generic;
using Core.Interfaces;

namespace Core.Services
{
    public class HandlersProvider
    {
        public List<Type> Handlers { get; } = new List<Type>();

        public HandlersProvider(ITypesProvider typesProvider)
        {
            foreach (var t in typesProvider.Types)
            {
                if (!t.IsClass) continue;
                if (t.IsAbstract) continue;

				if (!t.IsNested)
                {
	                if (!t.IsPublic) continue;
                }
                else
                {
	                if (!t.IsNestedPublic) continue;
                }

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
