using System;
using System.Collections.Generic;
using System.Linq;
using tBlabs.Cqrs.Core.Exceptions;
using tBlabs.Cqrs.Core.Interfaces;

namespace tBlabs.Cqrs.Core.Services
{
	public class MessageTypeProvider : IMessageTypeProvider
	{
		private Dictionary<string, Type> allMessagesNamesWithTheirTypes = new Dictionary<string, Type>();

		//public MessageTypeProvider(ITypesProvider typesProvider)
		//{
			//messageNameToType = typesProvider.Types
			//	.Where(t => t.IsClass && (t.IsPublic || t.IsNestedPublic) && !t.IsAbstract)
			//	.Where(t => typeof(IMessage).IsAssignableFrom(t) || typeof(IMessageWithStream).IsAssignableFrom(t))
			//	.ToDictionary(t => t.Name, t => t);
		//}
        public string[] MessagesList => allMessagesNamesWithTheirTypes.Keys.ToArray();

        public void RegisterMessages(Type[] types)
        {
            var messageNameToType = types
                .Where(t => t.IsClass && (t.IsPublic || t.IsNestedPublic) && !t.IsAbstract)
                .Where(t => typeof(IMessage).IsAssignableFrom(t) || typeof(IMessageWithStream).IsAssignableFrom(t))
                .ToDictionary(t => t.Name, t => t);

            foreach (var keyValuePair in messageNameToType)
            {
                allMessagesNamesWithTheirTypes.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        public Type GetByName(string name)
		{
			if (!allMessagesNamesWithTheirTypes.ContainsKey(name))
			{
				throw new MessageNotFoundException(name);
			}

			return allMessagesNamesWithTheirTypes[name];
		}
	}
}
