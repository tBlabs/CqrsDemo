using System.Threading.Tasks;
using tBlabs.Cqrs.Core.Interfaces;

namespace ModuleA
{
    public class DoubleValueQueryHandler : IQueryHandler<DoubleValueQuery, Task<int>>
    {
        public Task<int> Handle(DoubleValueQuery query)
        {
            return Task.FromResult(query.Value * 2);
        }
    }
}
