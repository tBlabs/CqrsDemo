using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;

namespace Core.Test
{
    public class MessageBusTests
    {
	    [Fact]
	    public void Should_resolve_message()
	    {
		    var messageAsJson = "{ 'name': 'SampleQuery', 'args': \"{ 'Foo': 'Bar' }\" }";
		 
		//	var sut = new MessageBus();
		}
    }
}
