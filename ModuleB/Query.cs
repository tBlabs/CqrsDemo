using tBlabs.Cqrs.Core.Interfaces;

namespace ModuleB
{
    public class Query : IQuery<int>
    {
        public int Value { get; set; }
    }
}