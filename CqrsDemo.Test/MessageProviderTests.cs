using Core.Cqrs;
using Core.Exceptions;
using Core.Services;
using FluentAssertions;
using Newtonsoft.Json;
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
	}
}
