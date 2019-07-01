using System.Threading.Tasks;
using tBlabs.Cqrs.Core.Interfaces;

namespace ModuleB
{
    public class CommandWithStreamHandler : ICommandHandler<CommandWithStream>
    {
        public Task Handle(CommandWithStream command)
        {
            return Task.CompletedTask;
        }
    }
}