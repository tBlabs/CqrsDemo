using System.Threading.Tasks;

namespace Core.Services
{
    public interface IMessageBus
    {
        Task<object> ExecuteFromJson(string messageAsJson);
    }
}