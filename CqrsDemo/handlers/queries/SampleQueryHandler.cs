using System;
using System.Collections.Generic;
using System.Text;

namespace CqrsDemo
{
    public class SampleQueryHandler : IQueryHandler<SampleQuery, string>
    {
        public string Handle(SampleQuery query)
        {
            Console.WriteLine("SampleQueryHandler.Handle(" + query.ToString());

            return query.Foo;
        }
    }
}
