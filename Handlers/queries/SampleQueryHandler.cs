using System;
using Core;
using Core.Cqrs;
using Messages.Dto;
using Messages.Queries;

namespace Handlers.Queries
{
	public class SampleQueryHandler : IQueryHandler<SampleQuery, SampleQueryResponse>
	{
		public SampleQueryResponse Handle(SampleQuery query)
		{
			Console.WriteLine("SampleQueryHandler.Handle(" + query.ToString() + ")");

			if (query.Foo == "Ex") throw new Exception("Exception");

			Console.WriteLine("return " + query.Foo);

			return new SampleQueryResponse { Baz = query.Foo };
		}
	}
}
