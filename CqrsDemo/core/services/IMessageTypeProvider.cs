using System;

namespace CqrsDemo
{
    public interface IMessageTypeProvider
    {
        Type GetByName(string name);
    }
}