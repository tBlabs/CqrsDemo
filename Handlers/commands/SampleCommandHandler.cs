using System;
using Core.Cqrs;
using Messages.Commands;

namespace Handlers.Commands
{
	public class SampleCommandHandler : ICommandHandler<SampleCommand>
	{
		public void Handle(SampleCommand command)
		{
			Console.WriteLine("SampleCommandHandler.Handle(" + command + ")");
			Console.WriteLine("(don't return nothing)");
		}
	}
}
