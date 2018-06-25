using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CqrsDemo
{
    class HandlerTypeProvider : IHandlerTypeProvider
    {
        private readonly Dictionary<Type, Type> messageTypeToHandlerType = new Dictionary<Type, Type>();

        public HandlerTypeProvider()
        {
            var thisAssemblyTypes = Assembly.GetExecutingAssembly().GetTypes();

            foreach (var t in thisAssemblyTypes)
            {
                if (!t.IsClass || !t.IsPublic || t.IsAbstract) continue;

                var classInterfaces = t.GetInterfaces();

                foreach (var i in classInterfaces)
                {
                    if (!i.IsGenericType) continue;

                    if ((i.GetGenericTypeDefinition() != typeof(ICommandHandler<>)) &&
                        (i.GetGenericTypeDefinition() != typeof(IQueryHandler<,>))) continue;

                    var message = i.GenericTypeArguments[0];

                    messageTypeToHandlerType.Add(message, t);
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
