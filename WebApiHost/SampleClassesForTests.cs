using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;

namespace WebApiHost
{
    public class Command : ICommand
    {
	    public int Value { get; set; }
    }

    public class Query : IQuery<int>
    {
	    public int Value { get; set; }
    }

    public class CommandWithStream : ICommandWithStream
    {
	    public Stream Stream { get; set; }
	    public string Foo { get; set; }
    }

    public class CommandHandler : ICommandHandler<Command>
    {
	    public Task Handle(Command command)
	    {
		    return Task.CompletedTask;
	    }
    }

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

    public class NotRegisteredMessage : IMessage
    { }
}
