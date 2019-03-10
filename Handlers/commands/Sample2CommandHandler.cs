using System;
using System.Threading.Tasks;
using Core.Cqrs;
using Messages.Commands;

namespace Handlers.Commands
{
    public class Sample2CommandHandler : ICommandHandler<Sample2Command>
    {
        public Task Handle(Sample2Command command)
        {
            Console.WriteLine("Sample2CommandHandler.Handle()");

			return Task.CompletedTask;
        }
    }
}
