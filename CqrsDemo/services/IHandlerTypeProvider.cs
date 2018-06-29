using System;

namespace Core
{
    public interface IHandlerTypeProvider
    {
        Type GetByMessageType(Type messageType);
    }
}