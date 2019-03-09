using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using FluentValidation;

namespace Core
{
    public class MessageBus : IMessageBus
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMessageProvider _messageProvider;
        private readonly IHandlerTypeProvider _handlerTypeProvider;

        public MessageBus(
            IServiceProvider serviceProvider, 
            IMessageProvider messageProvider, 
            IHandlerTypeProvider handlerTypeProvider)
        {
            this._serviceProvider = serviceProvider;
            this._messageProvider = messageProvider;
            this._handlerTypeProvider = handlerTypeProvider;
        }

        public object ExecuteFromJson(string messageAsJson) 
        {
            var message = _messageProvider.Resolve(messageAsJson);
            var handlerType = _handlerTypeProvider.GetByMessageType(message.GetType());
            dynamic handler = _serviceProvider.GetService(handlerType);

            switch (message)
            {
                case ICommand _:
                    handler.Handle((dynamic)message);
                    return null;              

                case IQueryBase _:
                    object returnValue = handler.Handle((dynamic)message);
                    return returnValue;

                default: throw new Exception("Invalid message type. Should be Command or Query.");
            }
        }
    }
}

