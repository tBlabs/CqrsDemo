using System;
using Core.Cqrs;
using Core.Exceptions;
using Core.Extensions;
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
			if (messageAsJson.IsNullOrEmpty())
			{
				throw new EmptyMessageException();
			}

			MessagePackage messagePackage = ExtractMessagePackage(messageAsJson);

			if (messagePackage.Args == null)
			{
				throw new NoMessageArgsException();
			}

			var messageType = _messageTypeProvider.GetByName(messagePackage.Name);

			var message = BuildMessage(messageType, messagePackage.Args);

			return message;
		}

		private IMessage BuildMessage(Type messageType, string messagePackageArgs)
		{
			try
			{
				return (IMessage)JsonConvert.DeserializeObject(messagePackageArgs, messageType);
			}
			catch (Exception)
			{
				throw new Exception($"Can not build message '{messageType.Name}' from these args: '{messagePackageArgs}'");
			}
		}

		private MessagePackage ExtractMessagePackage(string messageAsJson)
		{
			return JsonConvert.DeserializeObject<MessagePackage>(messageAsJson);
		}
	}
}
