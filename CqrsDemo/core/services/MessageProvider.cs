using System;
using CqrsDemo.utils;
using Newtonsoft.Json;

namespace CqrsDemo
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
            MessagePackage p = JsonConvert.DeserializeObject<MessagePackage>(messageAsJson);

            if (p.Args == null)
            {
                throw new Exception("No message package args detected. Should be at least empty string.");
            }

            var messageType = _messageTypeProvider.GetByName(p.Name);
    
            return (IMessage)JsonConvert.DeserializeObject(p.Args, messageType);
        }
    }
}
