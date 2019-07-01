using Microsoft.Extensions.DependencyInjection;
using tBlabs.Cqrs.Core.Extensions;
using tBlabs.Cqrs.Core.Interfaces;
using tBlabs.Cqrs.Core.Services;

namespace ModuleB
{
    public class Config : IModule
    {
        public void Register(IServiceCollection services)
        {
            services.AddHandler<CommandHandler>();
            services.AddHandler<CommandWithStreamHandler>();
            services.AddHandler<QueryHandler>();
        }

        public void RegisterCqrsStuff(IMessageTypeProvider messageTypeProvider, IHandlerTypeProvider handlerTypeProvider)
        {
            var types = this.GetType().Assembly.GetTypes();

            messageTypeProvider.RegisterMessages(types);
            handlerTypeProvider.RegisterHandlers(types);
        }
    }
}
