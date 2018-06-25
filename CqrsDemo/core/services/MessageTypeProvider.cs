using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CqrsDemo
{
    public class MessageTypeProvider : IMessageTypeProvider
    {
        private readonly Dictionary<string, Type> messageNameToType = new Dictionary<string, Type>();

        public MessageTypeProvider()
        {
            var thisAssemblyTypes = Assembly.GetExecutingAssembly().GetTypes();

            messageNameToType = thisAssemblyTypes
                .Where(t => t.IsClass && t.IsPublic && !t.IsAbstract)
                .Where(t => (typeof(IMessage)).IsAssignableFrom(t))
                .ToDictionary(t => t.Name, t => t);
        }

        public Type GetByName(string name)
        {
            return messageNameToType[name];
        }
    }
}
