using System;

namespace Core.Services
{
    public interface IHandlerTypeProvider
    {
        Type GetByMessageType(Type messageType);
    }
}