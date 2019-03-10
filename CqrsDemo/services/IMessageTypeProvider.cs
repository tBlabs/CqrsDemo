using System;

namespace Core.Services
{
    public interface IMessageTypeProvider
    {
        Type GetByName(string name);
    }
}