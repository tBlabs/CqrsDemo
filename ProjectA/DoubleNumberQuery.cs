using System.Threading.Tasks;
using tBlabs.Cqrs.Core.Interfaces;

namespace ModuleA
{
    public class DoubleValueQuery : IQuery<Task<int>>
    {
        public int Value { get; set; }
    }
}
