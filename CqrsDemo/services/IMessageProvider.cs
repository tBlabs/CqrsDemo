using System;

namespace Core
{
    public interface IMessageProvider
    {
        IMessage Resolve(string messageAsJson);
    }
}