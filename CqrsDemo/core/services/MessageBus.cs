using System;

namespace CqrsDemo
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

        public void Exe(string messageAsJson)
        {
            var message = _messageProvider.Resolve(messageAsJson);
            var handlerType = _handlerTypeProvider.GetByMessageType(message.GetType());
            dynamic handler = _serviceProvider.GetService(handlerType);

            switch (message)
            {
                case ICommand _:
                    handler.Handle((dynamic)message);
                    Console.WriteLine("Command executed");
                    break;

                case IQueryBase _:
                    object returnValue = handler.Handle((dynamic)message);
                    Console.WriteLine("Query returned " + returnValue);
                    break;
            }
        }
    }
}

