using System;
using System.Collections.Generic;
using System.Linq;
using Core.Cqrs;
using Core.Exceptions;
using Newtonsoft.Json;

namespace Core.Services
{
	public class MessageProvider : IMessageProvider
	{
		private readonly IMessageTypeProvider _messageTypeProvider;

		public MessageProvider(IMessageTypeProvider messageTypeProvider)
		{
			_messageTypeProvider = messageTypeProvider;
		}

		public IMessage Resolve(string messageAsJson)
		{
			var messageName = ExtractMessageName(messageAsJson);
			var messageType = _messageTypeProvider.GetByName(messageName);

			try
			{
				var dictionaryTypeForMessageToDeserialize = typeof(Dictionary<,>).MakeGenericType(typeof(string), messageType);

				var messageAsDictionary = (dynamic)JsonConvert.DeserializeObject(messageAsJson, dictionaryTypeForMessageToDeserialize);

				return (IMessage)messageAsDictionary[messageName];
			}
			catch (Exception)
			{
				throw new InvalidMessageException();
			}
		}

		private string ExtractMessageName(string messageAsJson) // TODO: can be done by regex
		{
			try
			{
				var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(messageAsJson);

				return dict.First().Key;
			}
			catch (Exception)
			{
				throw new InvalidMessageException();
			}
		}
	}
}
