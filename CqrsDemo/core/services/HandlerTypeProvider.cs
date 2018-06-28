using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CqrsDemo
{
    public class HandlerTypeProvider : IHandlerTypeProvider
    {
        private readonly Dictionary<Type, Type> messageTypeToHandlerType = new Dictionary<Type, Type>();

        public HandlerTypeProvider(IAssemblyTypesProvider thisAssemblyTypes)
        {
            foreach (var t in thisAssemblyTypes.Types)
            {
                if (!t.IsClass || !t.IsPublic || t.IsAbstract) continue;

                var classInterfaces = t.GetInterfaces();

                foreach (var i in classInterfaces)
                {
                    if (!i.IsGenericType) continue;

                    if ((i.GetGenericTypeDefinition() != typeof(ICommandHandler<>)) &&
                        (i.GetGenericTypeDefinition() != typeof(IQueryHandler<,>))) continue;

                    var messageType = i.GenericTypeArguments[0];

                    messageTypeToHandlerType.Add(messageType, t);
                }
            }
        }

        public Type GetByMessageType(Type messageType)
        {
            if (!messageTypeToHandlerType.ContainsKey(messageType))
            {
                throw new Exception("Message not found");
            }

            return messageTypeToHandlerType[messageType];
        }
    }
}
