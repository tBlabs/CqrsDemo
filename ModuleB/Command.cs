using tBlabs.Cqrs.Core.Interfaces;

namespace ModuleB
{
    public class Command : ICommand
    {
        public int Value { get; set; }
    }
}
