using System;
using System.Threading.Tasks;
using tBlabs.Cqrs.Core.Interfaces;
using tBlabs.Cqrs.Middleware;

namespace ModuleB
{
    public class CustomException : Exception, IHttpStatusCode
    {
        public int StatusCode => 444;
    }

    public class QueryHandler : IQueryHandler<Query, Task<int>>
    {
        public Task<int> Handle(Query query)
        {
            if (query.Value == 0)
            {
                throw new Exception("SomeExceptionMessage");
            }

            if (query.Value == (-1))
            {
                throw new CustomException();
            }

            return Task.FromResult(query.Value * 2);
        }
    }
}