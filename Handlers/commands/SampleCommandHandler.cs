using System;
using System.Threading.Tasks;
using Core.Cqrs;
using Messages.Commands;

namespace Handlers.Commands
{
	public class SampleCommandHandler : ICommandHandler<SampleCommand>
	{
		public Task Handle(SampleCommand command)
		{
			Console.WriteLine("SampleCommandHandler.Handle(" + command + ")");
			Console.WriteLine("(don't return nothing)");

			return Task.CompletedTask;
		}
	}
}
