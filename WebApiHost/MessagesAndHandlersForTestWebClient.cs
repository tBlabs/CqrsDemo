using System;
using System.IO;
using System.Threading.Tasks;
using Core.Interfaces;

namespace WebApiHost
{
	public class SampleQuery : IQuery<int>
	{
		public int Value { get; set; }
	}

	public class SampleQueryHandler : IQueryHandler<SampleQuery, Task<int>>
	{
		public Task<int> Handle(SampleQuery query)
		{
			return Task.FromResult(query.Value * 2);
		}
	}


	public class SampleCommand : ICommand
	{
		public string Foo { get; set; }
	}

	public class SampleCommandHandler : ICommandHandler<SampleCommand>
	{
		public Task Handle(SampleCommand command)
		{
			if (command.Foo == "throw")
			{
				throw new Exception("Some exception message from SampleCommand handler");
			}

			return Task.CompletedTask;
		}
	}


	public class CalculationsResponse
	{
		public int Sum { get; set; }
		public int Quotient { get; set; }
	}

	public class SampleQueryReturningObject : IQuery<CalculationsResponse>
	{
		public int A { get; set; }
		public int B { get; set; }
	}

	public class SampleQueryReturningObjectHandler : IQueryHandler<SampleQueryReturningObject, Task<CalculationsResponse>>
	{
		public Task<CalculationsResponse> Handle(SampleQueryReturningObject query)
		{
			return Task.FromResult(new CalculationsResponse
			{
				Sum = query.A + query.B,
				Quotient = query.A * query.B
			});
		}
	}


	public class SampleSaveFileCommand : ICommandWithStream
	{
		public Stream Stream { get; set; }
		public string SaveAs { get; set; }
	}

	public class SampleSaveFileCommandHandler : ICommandHandler<SampleSaveFileCommand>
	{
		public async Task Handle(SampleSaveFileCommand command)
		{
			using (var fileStream = File.Create(command.SaveAs))
			{
				await command.Stream.CopyToAsync(fileStream);
			}
		}
	}
}