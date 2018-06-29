using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class SampleQuery : IQuery<string>
    {
        public string Foo { get; set; }
    }
}
