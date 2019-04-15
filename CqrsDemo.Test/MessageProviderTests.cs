using System;
using System.IO;
using Core.Exceptions;
using Core.Interfaces;
using Core.Services;
using FluentAssertions;
using Moq;
using Shouldly;
using Xunit;

namespace Core.Test
{
	public class MessageProviderTest
	{
		public class TestQuery : IQuery<string>
		{
			public string Foo { get; set; }
		}

		public class TestCommandWithStream : ICommandWithStream
		{
			public Stream Stream { get; set; }
			public string Foo { get; set; }
		}

		private readonly IMessageProvider sut;

		public MessageProviderTest()
		{
			var messageTypeProviderMock = new Mock<IMessageTypeProvider>();
			messageTypeProviderMock.Setup(x => x.GetByName(nameof(TestQuery)))
				.Returns(typeof(TestQuery));
			messageTypeProviderMock.Setup(x => x.GetByName(nameof(TestCommandWithStream)))
				.Returns(typeof(TestCommandWithStream));

			sut = new MessageProvider(messageTypeProviderMock.Object);
		}

		[Fact]
		public void MessageProvider_should_throw_for_null_message()
		{
			string messageAsJson = null;

			Assert.Throws<InvalidMessageException>(() =>
			{
				sut.Resolve(messageAsJson);
			});
		}

		[Fact]
		public void MessageProvider_should_throw_for_empty_message()
		{
			var messageAsJson = "";

			Assert.Throws<InvalidMessageException>(() =>
			{
				sut.Resolve(messageAsJson);
			});
		}

		[Fact]
		public void MessageProvider_should_throw_for_invalid_input()
		{
			string messageAsJson = "invalid_message";

			Assert.Throws<InvalidMessageException>(() =>
			{
				sut.Resolve(messageAsJson);
			});
		}

		[Fact]
		public void MessageProvider_should_not_resolve_from_message_with_empty_args()
		{
			var messageAsJson = "{ 'TestQuery': { } }";

			IMessage message = sut.Resolve(messageAsJson);

			message.ShouldBeOfType<TestQuery>();
		}

		[Fact]
		public void MessageProvider_should_not_resolve_from_message_without_args()
		{
			var messageAsJson = "{ 'TestQuery' }";

			Assert.Throws<InvalidMessageException>(() =>
			{
				sut.Resolve(messageAsJson);
			});
		}

		[Fact]
		public void MessageProvider_should_resolve_from_valid_message()
		{
			var messageAsJson = "{ 'TestQuery': { 'Foo': 'Bar' } }";

			IMessage message = sut.Resolve(messageAsJson);

			message.ShouldBeOfType<TestQuery>();
			message.As<TestQuery>().Foo.ShouldBe("Bar");
		}
		
		[Fact]
		public void MessageProvider_should_not_resolve_from_valid_message_with_invalid_args_values()
		{
			var messageAsJson = "{ 'TestQuery': { 'Foo': { 'foo': 'bar' } }";

			Should.Throw<Exception>(() =>
			{
				sut.Resolve(messageAsJson);
			});
		}

		[Fact]
		public void MessageProvider_should_resolve_from_valid_message_with_invalid_args_keys()
		{
			var messageAsJson = "{ 'TestQuery': { 'BadKey': 'Bar' } }";

			var message = sut.Resolve(messageAsJson);

			message.ShouldBeOfType<TestQuery>();
			message.As<TestQuery>().Foo.ShouldBeNull();
		}

		[Fact]
		public void Should_provide_message_with_stream()
		{
			var messageAsJson = "{ 'TestCommandWithStream': { 'Foo': 'Bar' } }";
			var stream = new MemoryStream();

			var message = sut.Resolve(messageAsJson, stream);

			message.ShouldBeOfType<TestCommandWithStream>();
			message.As<TestCommandWithStream>().Stream.ShouldNotBeNull();
			message.As<TestCommandWithStream>().Foo.ShouldBe("Bar");
		}
	}
}
