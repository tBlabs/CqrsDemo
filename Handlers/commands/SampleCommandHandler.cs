using Core;
using System;
using Messages.Commands;

namespace Handlers
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
