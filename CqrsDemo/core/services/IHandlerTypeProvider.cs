using System;

namespace CqrsDemo
{
    public interface IHandlerTypeProvider
    {
        Type GetByMessageType(Type messageType);
    }
}