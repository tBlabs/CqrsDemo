namespace Core
{
    public interface IMessageBus
    {
        object Exe(string messageAsJson);
    }
}