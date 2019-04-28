using System;
using System.Collections.Generic;
using System.Linq;
using tBlabs.Cqrs.Core.Exceptions;
using tBlabs.Cqrs.Core.Interfaces;

namespace tBlabs.Cqrs.Core.Services
{
	public class MessageTypeProvider : IMessageTypeProvider
	{
		private readonly Dictionary<string, Type> messageNameToType;

		public MessageTypeProvider(ITypesProvider typesProvider)
		{
			messageNameToType = typesProvider.Types
				.Where(t => t.IsClass && (t.IsPublic || t.IsNestedPublic) && !t.IsAbstract)
				.Where(t => typeof(IMessage).IsAssignableFrom(t) || typeof(IMessageWithStream).IsAssignableFrom(t))
				.ToDictionary(t => t.Name, t => t);
		}

		public Type GetByName(string name)
		{
			if (!messageNameToType.ContainsKey(name))
			{
				throw new MessageNotFoundException(name);
			}

			return messageNameToType[name];
		}
	}
}
