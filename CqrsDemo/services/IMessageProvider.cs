using Core.Cqrs;

namespace Core.Services
{
    public interface IMessageProvider
    {
        IMessage Resolve(string messageAsJson);
    }
}