using System.IO;
using Core.Interfaces;

namespace Core.Services
{
    public interface IMessageProvider
    {
        IMessage Resolve(string messageAsJson, Stream stream = null);
    }
}