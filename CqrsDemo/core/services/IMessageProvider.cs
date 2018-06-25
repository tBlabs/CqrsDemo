using System;

namespace CqrsDemo
{
    public interface IMessageProvider
    {
        IMessage Resolve(string messageAsJson);
    }
}