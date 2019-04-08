using System;
using System.IO;
using Core.Exceptions;
using Core.Interfaces;
using Core.Services;
using FluentAssertions;
using Moq;
using Shouldly;
using WebApiHost;
using Xunit;

namespace Core.Test
{
	public class MessageProviderTest
	{
		public class SampleQuery : IQuery<string>
		{
			public string Foo { get; set; }
		}

		public class SampleCommandWithStream : ICommandWithStream
		{
			public Stream Stream { get; set; }
			public string Foo { get; set; }
		}

		private readonly IMessageProvider sut;

		public MessageProviderTest()
		{
			var messageTypeProviderMock = new Mock<IMessageTypeProvider>();
			messageTypeProviderMock.Setup(x => x.GetByName(nameof(SampleQuery))).Returns(typeof(SampleQuery));
			messageTypeProviderMock.Setup(x => x.GetByName(nameof(SampleCommandWithStream))).Returns(typeof(SampleCommandWithStream));

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
			var messageAsJson = "{ 'SampleQuery': { } }";

			IMessage message = sut.Resolve(messageAsJson);

			message.ShouldBeOfType<SampleQuery>();
		}

		[Fact]
		public void MessageProvider_should_not_resolve_from_message_without_args()
		{
			var messageAsJson = "{ 'SampleQuery' }";

			Assert.Throws<InvalidMessageException>(() =>
			{
				sut.Resolve(messageAsJson);
			});
		}

		[Fact]
		public void MessageProvider_should_resolve_from_valid_message()
		{
			var messageAsJson = "{ 'SampleQuery': { 'Foo': 'Bar' } }";

			IMessage message = sut.Resolve(messageAsJson);

			message.ShouldBeOfType<SampleQuery>();
			message.As<SampleQuery>().Foo.ShouldBe("Bar");
		}
		
		[Fact]
		public void MessageProvider_should_not_resolve_from_valid_message_with_invalid_args_values()
		{
			var messageAsJson = "{ 'SampleQuery': { 'Foo': { 'foo': 'bar' } }";

			Should.Throw<Exception>(() =>
			{
				sut.Resolve(messageAsJson);
			});
		}

		[Fact]
		public void MessageProvider2_should_resolve_from_valid_message_with_invalid_args_keys()
		{
			var messageAsJson = "{ 'SampleQuery': { 'Foo': 'bar' } }";

			var message = sut.Resolve(messageAsJson);

			message.ShouldBeAssignableTo<SampleQuery>();
			message.As<SampleQuery>().Foo.ShouldBe("bar");
		}

		[Fact]
		public void MessageProvider_should_resolve_from_valid_message_with_invalid_args_keys()
		{
			var messageAsJson = "{ 'SampleQuery': { 'BadKey': 'Bar' } }";

			var message = sut.Resolve(messageAsJson);

			message.ShouldBeOfType<SampleQuery>();
			message.As<SampleQuery>().Foo.ShouldBeNull();
		}

		[Fact]
		public void Should_provide_message_with_stream()
		{
			var messageAsJson = "{ 'SampleCommandWithStream': { 'Foo': 'Bar' } }";
			var stream = new MemoryStream();

			var message = sut.Resolve(messageAsJson, stream);

			message.ShouldBeOfType<SampleCommandWithStream>();
			message.As<SampleCommandWithStream>().Stream.ShouldNotBeNull();
			message.As<SampleCommandWithStream>().Foo.ShouldBe("Bar");
		}
	}
}
