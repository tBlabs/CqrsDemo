using System;
using System.IO;
using System.Threading.Tasks;
using tBlabs.Cqrs.Core.Interfaces;

namespace tBlabs.Cqrs.Core.Services
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
            _serviceProvider = serviceProvider;
            _messageProvider = messageProvider;
            _handlerTypeProvider = handlerTypeProvider;
        }

        public async Task<object> Execute(string messageAsJson, Stream stream = null) 
        {
            var message = _messageProvider.Resolve(messageAsJson, stream);
            var handlerType = _handlerTypeProvider.GetByMessageType(message.GetType());
            dynamic handler = _serviceProvider.GetService(handlerType);

            switch (message)
            {
                case ICommand _:
                    await handler.Handle((dynamic)message);
                    return null;              

                case IQueryBase _:
                    object returnValue = await handler.Handle((dynamic)message);
                    return returnValue;

                default: throw new Exception("Invalid message type. Should be Command or Query.");
            }
        }
    }
}

