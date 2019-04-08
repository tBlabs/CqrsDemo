using System;
using System.Collections.Generic;
using Core.Interfaces;

namespace Core.Services
{
    public class HandlerTypeProvider : IHandlerTypeProvider
    {
        private readonly Dictionary<Type, Type> messageTypeToHandlerType = new Dictionary<Type, Type>();

        public HandlerTypeProvider(ITypesProvider thisSolutionTypes)
        {
            foreach (var t in thisSolutionTypes.Types)
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

                    messageTypeToHandlerType.Add(messageType, t);
                }
            }
        }

        public Type GetByMessageType(Type messageType)
        {
            if (!messageTypeToHandlerType.ContainsKey(messageType))
            {
                throw new Exception($"Handler for message '{messageType.Name}' not found");
            }

            return messageTypeToHandlerType[messageType];
        }
    }
}
