using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using Shouldly;
using tBlabs.Cqrs.Core.Exceptions;
using tBlabs.Cqrs.Core.Services;
using tBlabs.Cqrs.Middlewares.Extensions;
using Xunit;

namespace tBlabs.Cqrs.Middlewares.Tests
{
	public class CqrsBusMiddlewareTests
	{
		[Fact]
		public void New_options_should_inherit_from_default()
		{
			var options = new CqrsBusMiddlewareOptions() {EndpointUrl = "abc"};
			options.EndpointUrl.ShouldBe("abc");
			options.MessageHeader.ShouldBe("Message");
		}

		[Fact]
		public async void MessageBus_should_be_called_for_regular_message()
		{
			// Given
			var sut = new CqrsBusMiddleware(null, CqrsBusMiddlewareOptions.Default);

			var messageBusMock = new Mock<IMessageBus>();
			messageBusMock.Setup(x => x.Execute(It.IsAny<string>(), null))
				.Returns(Task.FromResult((object)"value"));
			var httpContext = new DefaultHttpContext();

			httpContext.Request.Path = CqrsBusMiddlewareOptions.Default.EndpointUrl;
			httpContext.Response.Body = new MemoryStream();

			// When 
			await sut.InvokeAsync(httpContext, messageBusMock.Object);
			httpContext.Response.Body.Seek(0, SeekOrigin.Begin);

			// Then
			messageBusMock.Verify(x => x.Execute(It.IsAny<string>(), null), Times.Once);
			httpContext.Response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
			httpContext.Response.Body.ReadAsString().ShouldBe("\"value\"");
		}

		[Fact]
		public async void MessageBus_should_be_called_for_message_with_stream()
		{
			// Given
			var sut = new CqrsBusMiddleware(null, CqrsBusMiddlewareOptions.Default);

			var messageBusMock = new Mock<IMessageBus>();
			messageBusMock.Setup(x => x.Execute(It.IsAny<string>(), null))
				.Returns(Task.FromResult((object)"value"));
			var httpContext = new DefaultHttpContext();

			httpContext.Request.Path = CqrsBusMiddlewareOptions.Default.EndpointUrl;
			httpContext.Request.Headers.Add(CqrsBusMiddlewareOptions.Default.MessageHeader, "some_message");
			httpContext.Request.Body = new MemoryStream();

			// When 
			await sut.InvokeAsync(httpContext, messageBusMock.Object);
			httpContext.Response.Body.Seek(0, SeekOrigin.Begin);

			// Then
			messageBusMock.Verify(x => x.Execute("some_message", It.IsAny<Stream>()), Times.Once);
			httpContext.Response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
			httpContext.Request.Body.ShouldBeOfType<MemoryStream>();
		}

		[Fact]
		public async void Http_404_should_be_returned_for_not_registered_message()
		{
			// Given
			var sut = new CqrsBusMiddleware(null, CqrsBusMiddlewareOptions.Default);

			var messageBusMock = new Mock<IMessageBus>();
			messageBusMock.Setup(x => x.Execute(It.IsAny<string>(), null))
				.Throws(new MessageNotFoundException("some_message"));
			var httpContext = new DefaultHttpContext();

			httpContext.Request.Path = CqrsBusMiddlewareOptions.Default.EndpointUrl;
			httpContext.Response.Body = new MemoryStream();

			// When 
			await sut.InvokeAsync(httpContext, messageBusMock.Object);
			httpContext.Response.Body.Seek(0, SeekOrigin.Begin);

			// Then
			messageBusMock.Verify(x => x.Execute(It.IsAny<string>(), null), Times.Once);
			httpContext.Response.StatusCode.ShouldBe((int)HttpStatusCode.NotFound);
			httpContext.Response.Body.ReadAsString().ShouldBe("Message 'some_message' not found");
		}

		[Fact]
		public async void Http_500_should_be_returned_for_handler_exception()
		{
			// Given
			var sut = new CqrsBusMiddleware(null, CqrsBusMiddlewareOptions.Default);

			var messageBusMock = new Mock<IMessageBus>();
			messageBusMock.Setup(x => x.Execute(It.IsAny<string>(), null))
				.Throws(new Exception("some exception message"));
			var httpContext = new DefaultHttpContext();

			httpContext.Request.Path = CqrsBusMiddlewareOptions.Default.EndpointUrl;
			httpContext.Response.Body = new MemoryStream();

			// When 
			await sut.InvokeAsync(httpContext, messageBusMock.Object);
			httpContext.Response.Body.Seek(0, SeekOrigin.Begin);

			// Then
			messageBusMock.Verify(x => x.Execute(It.IsAny<string>(), null), Times.Once);
			httpContext.Response.StatusCode.ShouldBe((int)HttpStatusCode.InternalServerError);
			httpContext.Response.Body.ReadAsString().ShouldBe("some exception message");
		}

		[Fact]
		public async void Next_middleware_should_be_called_if_path_was_different_than_CqrsBus()
		{
			// Given
			var nextMock = new Mock<RequestDelegate>();

			var sut = new CqrsBusMiddleware(nextMock.Object, CqrsBusMiddlewareOptions.Default);

			var context = new DefaultHttpContext();
			context.Request.Path = "/NotCqrsBus";

			// When
			await sut.InvokeAsync(context, null);

			// Then
			nextMock.Verify(x => x(It.IsAny<HttpContext>()), Times.Once);
		}

		[Fact]
		public async void HttpContext_write_read_proof()
		{
			HttpContext httpContext = new DefaultHttpContext();
			httpContext.Response.Body = new MemoryStream();

			await httpContext.Response.WriteAsync("test");

			httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
			httpContext.Response.Body.ReadAsString().ShouldBe("test");
		}
	}
}
