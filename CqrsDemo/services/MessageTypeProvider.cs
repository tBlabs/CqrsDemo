using System;
using System.Collections.Generic;
using System.Linq;
using Core.Cqrs;
using Core.Exceptions;

namespace Core.Services
{
	public class MessageTypeProvider : IMessageTypeProvider
	{
		private readonly Dictionary<string, Type> messageNameToType;

		public MessageTypeProvider(ISolutionTypesProvider typesProvider)
		{
			messageNameToType = typesProvider.Types
				.Where(t => t.IsClass && t.IsPublic && !t.IsAbstract)
				.Where(t => (typeof(IMessage)).IsAssignableFrom(t))
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
