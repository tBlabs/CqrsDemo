using System;
using System.Threading.Tasks;
using tBlabs.Cqrs.Core.Interfaces;

namespace ModuleB
{
    public class QueryHandler : IQueryHandler<Query, Task<int>>
    {
        public Task<int> Handle(Query query)
        {
            if (query.Value == 0)
            {
                throw new Exception("SomeExceptionMessage");
            }

            return Task.FromResult(query.Value * 2);
        }
    }
}