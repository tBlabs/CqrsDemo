using System.IO;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IMessageBus
    {
        Task<object> Execute(string messageAsJson, Stream stream = null);
    }
}