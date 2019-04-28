using System.IO;
using tBlabs.Cqrs.Core.Interfaces;

namespace tBlabs.Cqrs.Core.Services
{
    public interface IMessageProvider
    {
        IMessage Resolve(string messageAsJson, Stream stream = null);
    }
}