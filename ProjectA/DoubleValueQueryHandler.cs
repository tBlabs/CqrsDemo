using System.Threading.Tasks;
using tBlabs.Cqrs.Core.Interfaces;

namespace ModuleA
{
    public class DoubleValueQueryHandler : IQueryHandler<DoubleValueQuery, Task<int>>
    {
        private readonly DoubleItService _doubler;

        public DoubleValueQueryHandler(DoubleItService doubler)
        {
            _doubler = doubler;
        }

        public Task<int> Handle(DoubleValueQuery query)
        {
            var doubledValue = _doubler.Double(query.Value);

            return Task.FromResult(doubledValue);
        }
    }
}
