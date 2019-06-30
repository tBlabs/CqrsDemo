using System;

namespace tBlabs.Cqrs.Core.Services
{
    public interface IMessageTypeProvider
    {
        void RegisterMessages(Type[] types);
        Type GetByName(string name);
        string[] MessagesList { get; }
    }
}