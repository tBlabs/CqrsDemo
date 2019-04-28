using System;

namespace tBlabs.Cqrs.Core.Services
{
    public interface IMessageTypeProvider
    {
        Type GetByName(string name);
    }
}