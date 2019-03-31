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
		private readonly IMessageProvider messageProvider2;

		public MessageProviderTest()
		{
			var thisProjectTypes = new SolutionTypesProvider();
			var messageTypeProvider = new MessageTypeProvider(thisProjectTypes);
			messageProvider = new MessageProvider(messageTypeProvider);
			messageProvider2 = new MessageProvider(messageTypeProvider);
		}

		[Fact]
		public void MessageProvider_should_throw_for_null_message()
		{
			string messageAsJson = null;

			Assert.Throws<InvalidMessageException>(() =>
			{
				messageProvider.Resolve(messageAsJson);
			});
		}

		[Fact]
		public void MessageProvider_should_throw_for_empty_message()
		{
			var messageAsJson = "";

			Assert.Throws<InvalidMessageException>(() =>
			{
				messageProvider.Resolve(messageAsJson);
			});
		}

		[Fact]
		public void MessageProvider_should_throw_for_invalid_input()
		{
			string messageAsJson = "invalid_message";

			Assert.Throws<InvalidMessageException>(() =>
			{
				messageProvider.Resolve(messageAsJson);
			});
		}

		[Fact]
		public void MessageProvider_should_not_resolve_from_message_with_empty_args()
		{
			var messageAsJson = "{ 'SampleQuery': { } }";

			IMessage message = messageProvider.Resolve(messageAsJson);

			message.ShouldBeOfType<SampleQuery>();
		}

		[Fact]
		public void MessageProvider_should_not_resolve_from_message_without_args()
		{
			var messageAsJson = "{ 'SampleQuery' }";

			Assert.Throws<InvalidMessageException>(() =>
			{
				messageProvider.Resolve(messageAsJson);
			});
		}

		[Fact]
		public void MessageProvider_should_resolve_from_valid_message()
		{
			var messageAsJson = "{ 'SampleQuery': { 'Foo': 'Bar' } }";

			IMessage message = messageProvider.Resolve(messageAsJson);

			message.ShouldBeOfType<SampleQuery>();
			message.As<SampleQuery>().Foo.ShouldBe("Bar");
		}
		
		[Fact]
		public void MessageProvider_should_not_resolve_from_valid_message_with_invalid_args_values()
		{
			var messageAsJson = "{ 'SampleQuery': { 'Foo': { 'foo': 'bar' } }";

			Should.Throw<Exception>(() =>
			{
				messageProvider.Resolve(messageAsJson);
			});
		}

		[Fact]
		public void MessageProvider2_should_resolve_from_valid_message_with_invalid_args_keys()
		{
			var messageAsJson = "{ 'SampleQuery': { 'Foo': 'bar' } }";

			var message = messageProvider2.Resolve(messageAsJson);

			message.ShouldBeAssignableTo<SampleQuery>();
			message.As<SampleQuery>().Foo.ShouldBe("bar");
		}

		[Fact]
		public void MessageProvider_should_resolve_from_valid_message_with_invalid_args_keys()
		{
			var messageAsJson = "{ 'SampleQuery': { 'BadKey': 'Bar' } }";

			var message = messageProvider.Resolve(messageAsJson);

			message.ShouldBeOfType<SampleQuery>();
			message.As<SampleQuery>().Foo.ShouldBeNull();
		}
	}
}
