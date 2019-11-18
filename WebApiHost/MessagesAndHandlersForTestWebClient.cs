using System;
using System.Collections.Generic;
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

    public class SampleMoreComplicatedCommand : ICommand
    {
        public string String { get; set; }
        public int Int { get; set; }
        public long Long { get; set; }
        public bool Boolean { get; set; }
        public IEnumerable<string> EnumerableString { get; set; }
        public IEnumerable<int> EnumerableInt { get; set; }
        public IEnumerable<long> EnumerableLong { get; set; }
    }

    public class SampleMoreComplicatedCommandHandler : ICommandHandler<SampleMoreComplicatedCommand>
    {
        public Task Handle(SampleMoreComplicatedCommand command)
        {
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

    public class SampleUploadFileCommandHandler : ICommandHandler<SampleUploadFileCommand>
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
           // var s = new MemoryStream(Encoding.ASCII.GetBytes("Query.Value=" + query.Value)); // Do not use `using` here. Stream will be disposed in MessageBus (this is bad, I know..)
            string path = @"C:\Users\tbudre01\Desktop\testfile3.xls";      
            FileStream fs = File.OpenRead(path);
            Stream ms = new MemoryStream();
            fs.CopyTo(ms);
    //        ms.Position = 0;

            return await Task.FromResult(ms);
        }
    }

    public class SampleDownloadFileQuery : IQuery<Task<Stream>>
    {
        public int Value { get; set; }
    }
}