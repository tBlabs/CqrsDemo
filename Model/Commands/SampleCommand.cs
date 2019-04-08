using Core.Interfaces;

namespace Messages.Commands
{
    public class SampleCommand : ICommand
    {
        public string Foo { get; set; }

        public override string ToString()
        {
            return "SampleCommand: Foo="+Foo;
        }
    }
}
