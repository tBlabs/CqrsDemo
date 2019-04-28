using System;

namespace tBlabs.Cqrs.Core.Services
{
    public interface IHandlerTypeProvider
    {
        Type GetByMessageType(Type messageType);
    }
}