using System;
using Core.utils;
using Newtonsoft.Json;

namespace Core
{
	public class MessageProvider : IMessageProvider
	{
		private readonly IMessageTypeProvider _messageTypeProvider;

		public MessageProvider(IMessageTypeProvider messageTypeProvider)
		{
			this._messageTypeProvider = messageTypeProvider;
		}

		public IMessage Resolve(string messageAsJson)
		{
			if (messageAsJson.IsNullOrEmpty())
			{
				throw new Exception("Message can not be empty");
			}

			//try
			//{
				MessagePackage p = JsonConvert.DeserializeObject<MessagePackage>(messageAsJson);

				if (p.Args == null)
				{
					throw new Exception("No message package args detected. Should be at least an empty string."); // TODO: custom exception
				}

				var messageType = _messageTypeProvider.GetByName(p.Name);

				return (IMessage)JsonConvert.DeserializeObject(p.Args, messageType);
			//}
			//// TODO: catch custom exception
			//catch (Exception)
			//{
			//	throw new Exception("Invalid json input: " + messageAsJson);
			//}
		}
	}
}
