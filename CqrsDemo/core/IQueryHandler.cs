using System;
using System.Collections.Generic;
using System.Text;

namespace CqrsDemo
{
    public interface IQueryHandler<in TQuery, out TResult> where TQuery : IQueryBase
    {
        TResult Handle(TQuery query);
    }
}
