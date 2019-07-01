using System.IO;
using tBlabs.Cqrs.Core.Interfaces;

namespace ModuleB
{
    public class CommandWithStream : ICommandWithStream
    {
        public Stream Stream { get; set; }
        public string Foo { get; set; }
    }
}