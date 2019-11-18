using Microsoft.Extensions.DependencyInjection;
using tBlabs.Cqrs.Core.Extensions;
using tBlabs.Cqrs.Core.Interfaces;
using tBlabs.Cqrs.Core.Services;

namespace WebApiHost
{
    public class Config : IModule
    {
        public void Register(IServiceCollection services)
        {
            services.AddHandler<SampleCommandHandler>();
            services.AddHandler<SampleQueryHandler>();
            services.AddHandler<SampleQueryReturningObjectHandler>();
            services.AddHandler<SampleUploadFileCommandHandler>();
            services.AddHandler<SampleDownloadFileQueryHandler>();
            services.AddHandler<SampleMoreComplicatedCommandHandler>();
        }

        public void RegisterCqrsStuff(IMessageTypeProvider messageTypeProvider, IHandlerTypeProvider handlerTypeProvider)
        {
            var types = this.GetType().Assembly.GetTypes();

            messageTypeProvider.RegisterMessages(types);
            handlerTypeProvider.RegisterHandlers(types);
        }
    }
}
