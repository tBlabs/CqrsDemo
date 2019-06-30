using System;

namespace tBlabs.Cqrs.Core.Services
{
    public interface IHandlerTypeProvider
    {
        Type GetByMessageType(Type messageType);
        void RegisterHandlers(Type[] types);
        Type[] Handlers { get; }
    }
}