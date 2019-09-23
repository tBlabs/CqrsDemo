using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using tBlabs.Cqrs.Core.Interfaces;

namespace WebApiHost
{
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


    public class CalculationsResult
    {
        public int Sum { get; set; }
        public int Quotient { get; set; }
    }

    public class SampleQueryReturningObject : IQuery<CalculationsResult>
    {
        public int A { get; set; }
        public int B { get; set; }
    }

    public class SampleQueryReturningObjectHandler : IQueryHandler<SampleQueryReturningObject, Task<CalculationsResult>>
    {
        public Task<CalculationsResult> Handle(SampleQueryReturningObject query)
        {
            return Task.FromResult(new CalculationsResult
            {
                Sum = query.A + query.B,
                Quotient = query.A * query.B
            });
        }
    }

    public class SampleUploadFileCommand : ICommandWithStream
    {
        public Stream Stream { get; set; }
        public string Value { get; set; }
    }

    public class SampleSaveFileCommandHandler : ICommandHandler<SampleUploadFileCommand>
    {
        public async Task Handle(SampleUploadFileCommand command)
        {
            using (var fileStream = File.Create(command.Value))
            {
                await command.Stream.CopyToAsync(fileStream);
            }
        }
    }

    public class SampleDownloadFileQueryHandler : IQueryHandler<SampleDownloadFileQuery, Task<Stream>>
    {
        public async Task<Stream> Handle(SampleDownloadFileQuery query)
        {
            var s = new MemoryStream(Encoding.ASCII.GetBytes("Query.Value=" + query.Value)); // Do not use `using` here. Stream will be disposed in MessageBus (this is bad, I know..)

            return await Task.FromResult(s);
        }
    }

    public class SampleDownloadFileQuery : IQuery<Task<Stream>>
    {
        public int Value { get; set; }
    }
}