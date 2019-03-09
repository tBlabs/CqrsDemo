namespace Core
{
    public interface IMessageBus
    {
        object ExecuteFromJson(string messageAsJson);
    }
}