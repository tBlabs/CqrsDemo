using System;
using Core.Cqrs;
using Messages.Commands;

namespace Handlers.Commands
{
    public class Sample2CommandHandler : ICommandHandler<Sample2Command>
    {
        public void Handle(Sample2Command command)
        {
            Console.WriteLine("Sample2CommandHandler.Handle()");
        }
    }
}
