using System;

namespace CqrsDemo
{
    public class SampleCommandHandler : ICommandHandler<SampleCommand>
    {
        public void Handle(SampleCommand command)
        {
            Console.WriteLine("SampleCommandHandler.Handle("+command);
        }
    }
}
