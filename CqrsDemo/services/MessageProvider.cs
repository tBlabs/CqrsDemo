using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using tBlabs.Cqrs.Core.Exceptions;
using tBlabs.Cqrs.Core.Interfaces;

namespace tBlabs.Cqrs.Core.Services
{
	public class MessageProvider : IMessageProvider
	{
		private readonly IMessageTypeProvider _messageTypeProvider;

		public MessageProvider(IMessageTypeProvider messageTypeProvider)
		{
			_messageTypeProvider = messageTypeProvider;
		}

		public IMessage Resolve(string messageAsJson, Stream stream = null)
		{
			var messageName = ExtractMessageName(messageAsJson);
			var messageType = _messageTypeProvider.GetByName(messageName);

			try
			{
				var dictionaryTypeForMessageToDeserialize = typeof(Dictionary<,>).MakeGenericType(typeof(string), messageType);

				var messageAsDictionary = (dynamic)JsonConvert.DeserializeObject(messageAsJson, dictionaryTypeForMessageToDeserialize);

				if (stream == null)
				{
					return (IMessage)messageAsDictionary[messageName];
				}
				else
				{
					var message = (IMessageWithStream)messageAsDictionary[messageName];
					message.Stream = stream;
					return message;
				}
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
