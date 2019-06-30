using System;
using System.Collections.Generic;
using System.Linq;
using tBlabs.Cqrs.Core.Interfaces;

namespace tBlabs.Cqrs.Core.Services
{
    public interface IHandlersProvider
    {
        List<Type> Handlers { get; }
        void RegisterHandlers(Type[] types);
    }

    public class HandlersProvider : IHandlersProvider
    {
		public List<Type> Handlers { get; } = new List<Type>();

		public void RegisterHandlers(Type[] types)
		{
			foreach (var t in types
				.Where(x => x.IsClass && !x.IsAbstract)
				.Where(x => x.IsPublic || x.IsNestedPublic))
			{
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

