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
			return Task.CompletedTask;
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