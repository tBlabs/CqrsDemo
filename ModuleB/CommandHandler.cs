using System.Threading.Tasks;
using tBlabs.Cqrs.Core.Interfaces;

namespace ModuleB
{
    public class CommandHandler : ICommandHandler<Command>
    {
        public Task Handle(Command command)
        {
            return Task.CompletedTask;
        }
    }
}
