using Microsoft.Extensions.DependencyInjection;
using tBlabs.Cqrs.Core.Services;

namespace tBlabs.Cqrs.Core.Interfaces
{
    public interface IModule
    {

        void Register(IServiceCollection services);

        void RegisterCqrsStuff(IMessageTypeProvider messageTypeProvider,
            IHandlerTypeProvider handlerTypeProvider);
    }
}
