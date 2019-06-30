using Microsoft.Extensions.DependencyInjection;
using tBlabs.Cqrs.Core.Interfaces;
using tBlabs.Cqrs.Core.Services;

namespace FeatureX
{
    public class ConfigModule : IModule
    {
        public void Register(IServiceCollection services)
        {
            services.AddTransient<FeatureXSampleCommandHandler>();
            services.AddTransient<FeatureXSampleQueryHandler>();
        }

        public void RegisterCqrsStuff(
            IMessageTypeProvider messageTypeProvider,
            IHandlerTypeProvider handlerTypeProvider)
        {
            var types = this.GetType().Assembly.GetTypes();

            messageTypeProvider.RegisterMessages(types);
            handlerTypeProvider.RegisterHandlers(types);
        }
    }
}
