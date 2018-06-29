using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Core
{
    public class MessageTypeProvider : IMessageTypeProvider
    {
        private readonly Dictionary<string, Type> messageNameToType = new Dictionary<string, Type>();

        public MessageTypeProvider(IAssemblyTypesProvider thisAssemblyTypes)
        {
            messageNameToType = thisAssemblyTypes.Types
                .Where(t => t.IsClass && t.IsPublic && !t.IsAbstract)
                .Where(t => (typeof(IMessage)).IsAssignableFrom(t))
                .ToDictionary(t => t.Name, t => t);
        }

        public Type GetByName(string name)
        {
            if (!messageNameToType.ContainsKey(name))
            {
                throw new Exception($"Message {name} not found");
            }

            return messageNameToType[name];
        }
    }
}
