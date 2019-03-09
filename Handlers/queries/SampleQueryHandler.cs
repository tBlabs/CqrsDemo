using System;
using Core;
using Messages.Dto;
using Messages.Queries;

namespace Handlers.Queries
{
	public class SampleQueryHandler : IQueryHandler<SampleQuery, SampleQueryResponse>
	{
		public SampleQueryResponse Handle(SampleQuery query)
		{
			Console.WriteLine("SampleQueryHandler.Handle(" + query.ToString() + ")");
			Console.WriteLine("return " + query.Foo);

			return new SampleQueryResponse { Baz = query.Foo };
		}
	}
}
