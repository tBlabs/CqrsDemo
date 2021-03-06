﻿using Shouldly;
using tBlabs.Cqrs.Core.Interfaces;
using WebApiHost.Tests.Helpers;
using Xunit;

namespace WebApiHost.Tests
{
    public class MessageExtensionsTests
    {
	    private class Message : IQuery<int>
	    {
		    public string Foo { get; set; }
	    }

	    [Fact]
	    public void Message_ToJson_extension_test()
	    {
		    var msg = new Message() { Foo = "bar" };

		    var json = msg.ToJson();

		    json.ShouldBe("{\"Message\":{\"Foo\":\"bar\"}}");
	    }
	}
}
