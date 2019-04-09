using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;

namespace WebApiHost
{
	public class TestQuery : IQuery<int>
	{
		public int Value { get; set; }
	}

	public class TestQueryHandler : IQueryHandler<TestQuery, Task<int>>
	{
		public Task<int> Handle(TestQuery query)
		{
			return Task.FromResult(query.Value * 2);
		}
	}

	public class TestCommand : ICommand
	{
		public string Foo { get; set; }
	}

	public class TestCommandHandler : ICommandHandler<TestCommand>
	{
		public Task Handle(TestCommand command)
		{
			return Task.CompletedTask;
		}
	}

	public class SaveFileCommand : ICommandWithStream
	{
		public Stream Stream { get; set; }
		public string SaveAs { get; set; }
	}

	public class SaveFileCommandHandler : ICommandHandler<SaveFileCommand>
	{
		public Task Handle(SaveFileCommand command)
		{
			using (var fileStream = File.Create(command.SaveAs))
			{
				command.Stream.CopyTo(fileStream);
			}

			return Task.CompletedTask;
		}
	}
}
