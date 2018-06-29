using Core;
using System;

namespace Handlers
{
    public class SampleCommandHandler : ICommandHandler<SampleCommand>
    {
        public void Handle(SampleCommand command)
        {
            Console.WriteLine("SampleCommandHandler.Handle("+command);
        }
    }
}
