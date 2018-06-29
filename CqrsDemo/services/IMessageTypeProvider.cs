using System;

namespace Core
{
    public interface IMessageTypeProvider
    {
        Type GetByName(string name);
    }
}