using System;
using System.Collections.Generic;
using System.Linq;
using tBlabs.Cqrs.Core.Exceptions;
using tBlabs.Cqrs.Core.Interfaces;

namespace tBlabs.Cqrs.Core.Services
{
    public class HandlerTypeProvider : IHandlerTypeProvider
    {
        private readonly Dictionary<Type, Type> allMessageTypeToHandlerType = new Dictionary<Type, Type>();

        public Type[] Handlers => allMessageTypeToHandlerType.Values.ToArray();

        public void RegisterHandlers(Type[] types)
        {
            foreach (var t in types)
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

				var classInterfaces = t.GetInterfaces();

                foreach (var i in classInterfaces)
                {
                    if (!i.IsGenericType) continue;

                    if (i.GetGenericTypeDefinition() != typeof(ICommandHandler<>) &&
                        i.GetGenericTypeDefinition() != typeof(IQueryHandler<,>)) continue;

                    var messageType = i.GenericTypeArguments[0];

                    allMessageTypeToHandlerType.Add(messageType, t);
                }
            }
        }

        public Type GetByMessageType(Type messageType)
        {
            if (!allMessageTypeToHandlerType.ContainsKey(messageType))
            {
	            throw new HandlerNotFoundException(messageType);
            }

            return allMessageTypeToHandlerType[messageType];
        }
    }
}
