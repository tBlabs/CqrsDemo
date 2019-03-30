using System;
using System.Collections.Generic;
using System.Linq;
using Core.Cqrs;
using Core.Exceptions;
using Core.Services;
using FluentAssertions;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace Core.Test
{
	public class SampleQuery : IQuery<string>
	{
		public string Foo { get; set; }
	}

	public class MessageProviderTest
	{
		private readonly IMessageProvider messageProvider;

		public MessageProviderTest()
		{
			var thisProjectTypes = new SolutionTypesProvider();
			var messageTypeProvider = new MessageTypeProvider(thisProjectTypes);
			messageProvider = new MessageProvider(messageTypeProvider);
		}

		[Fact]
		public void MessageProvider_should_throw_for_null_message()
		{
			string messageAsJson = null;

			Assert.Throws<EmptyMessageException>(() =>
			{
				messageProvider.Resolve(messageAsJson);
			});
		}

		[Fact]
		public void MessageProvider_should_throw_for_empty_message()
		{
			var messageAsJson = "";

			Assert.Throws<EmptyMessageException>(() =>
			{
				messageProvider.Resolve(messageAsJson);
			});
		}

		[Fact]
		public void MessageProvider_should_throw_for_invalid_input()
		{
			string messageAsJson = "invalid_message";

			Assert.Throws<JsonReaderException>(() =>
			{
				messageProvider.Resolve(messageAsJson);
			});
		}

		[Fact]
		public void MessageProvider_should_not_resolve_from_message_with_empty_args()
		{
			var messageAsJson = "{ 'name': 'SampleQuery', 'args': '' }";

			IMessage message = messageProvider.Resolve(messageAsJson);

			message.Should().BeNull();
		}

		[Fact]
		public void MessageProvider_should_not_resolve_from_message_without_args()
		{
			var messageAsJson = "{ 'name': 'SampleQuery' }";

			Assert.Throws<NoMessageArgsException>(() =>
			{
				messageProvider.Resolve(messageAsJson);
			});
		}

		[Fact]
		public void MessageProvider_should_resolve_from_valid_message()
		{
			var messageAsJson = "{ 'name': 'SampleQuery', 'args': \"{ 'Foo': 'Bar' }\" }";

			IMessage message = messageProvider.Resolve(messageAsJson);

			message.Should().BeOfType<SampleQuery>();
			message.As<SampleQuery>().Foo.Should().Be("Bar");
		}

		[Fact]
		public void MessageProvider_should_not_resolve_from_valid_message_with_invalid_args_values()
		{
			var messageAsJson = "{ 'name': 'SampleQuery', 'args': \"{ 'Foo': { 'foo': 'bar' } }\" }";

			Should.Throw<Exception>(() =>
			{
				messageProvider.Resolve(messageAsJson);
			});
		}

		[Fact]
		public void Json_test()
		{
			var objTxt = @"{
				'foo': 123,
				'baz': 4,
				'obj': { 'foo': 'barrr' },
				'tab': [1,2,3]
			}";

			var obj = JsonConvert.DeserializeObject<Obj>(objTxt);
			obj.Foo.ShouldBe("123");
			obj.Baz.ShouldBe(4);
			obj.obj.ShouldBeOfType<Obj2>();
			obj.obj2.ShouldBeNull();
			obj.Tab.Count().ShouldBe(3);
		}

		[Fact]
		public void MessageProvider_should_resolve_from_valid_message_with_invalid_args_keys()
		{
			var messageAsJson = "{ 'name': 'SampleQuery', 'args': \"{ 'BadKey': 'Bar' }\" }";

			var message = messageProvider.Resolve(messageAsJson);

			message.ShouldBeAssignableTo<SampleQuery>();
			(message as SampleQuery)?.Foo.ShouldBeNull();
		}
	}

	public class Obj
	{
		public string Foo { get; set; }
		public int Baz { get; set; }
		public Obj2 obj { get; set; }
		public Obj2 obj2 { get; set; }
		public IEnumerable<int> Tab { get; set; }
	}
	public class Obj2
	{
		public string Foo { get; set; }
	}
}
