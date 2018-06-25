namespace CqrsDemo
{
    public interface IMessageBus
    {
        void Exe(string messageAsJson);
    }
}