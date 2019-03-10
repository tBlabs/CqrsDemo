using Core.Cqrs;

namespace Messages.Commands
{
    public class Sample2Command : ICommand
    {
        public string Foo { get; set; }
    }
}
