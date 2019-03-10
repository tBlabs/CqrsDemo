namespace Core.Services
{
    public interface IMessageBus
    {
        object ExecuteFromJson(string messageAsJson);
    }
}