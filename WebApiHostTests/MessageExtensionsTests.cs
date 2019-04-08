using Core.Interfaces;
using Shouldly;
using WebApiHostTests.Helpers;
using Xunit;

namespace WebApiHostTests
{
    public class MessageExtensionsTests
    {
	    private class SampleQuery : IQuery<int>
	    {
		    public string Foo { get; set; }
	    }

	    [Fact]
	    public void Message_ToJson_extension_test()
	    {
		    var msg = new SampleQuery() { Foo = "bar" };

		    var json = msg.ToJson();

		    json.ShouldBe("{\"SampleQuery\":{\"Foo\":\"bar\"}}");
	    }
	}
}
