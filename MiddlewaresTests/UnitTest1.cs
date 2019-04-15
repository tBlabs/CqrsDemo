using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Core.Services;
using Microsoft.AspNetCore.Http;
using Middlewares;
using Middlewares.Extensions;
using Moq;
using Shouldly;
using Xunit;

namespace MiddlewaresTests
{
	public class CqrsBusMiddlewareTests
	{
		[Fact]
		public async void MessageBus_should_be_called_for_regular_message()
		{
			// Given
			var sut = new CqrsBusMiddleware(null, CqrsBusMiddlewareOptions.Default);

			var messageBusMock = new Mock<IMessageBus>();
			//messageBusMock.Setup(x => x.Execute(It.IsAny<string>(), null))
			//	.Returns(Task.FromResult((object)"value"));
			var httpContext = new DefaultHttpContext();

			httpContext.Request.Path = CqrsBusMiddlewareOptions.Default.EndpointUrl;
			// httpContext.Response.Body = new MemoryStream();

			// When 
			await sut.InvokeAsync(httpContext, messageBusMock.Object);

			// Then
			messageBusMock.Verify(x => x.Execute(It.IsAny<string>(), null), Times.Once);
			httpContext.Response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
			// ?!?!?! httpContext.Response.Body.ReadAsString().ShouldBe("\"value\"");
		}

		[Fact]
		public async void MessageBus_should_be_called_for_message_with_stream()
		{
			// Given
			var sut = new CqrsBusMiddleware(null, CqrsBusMiddlewareOptions.Default);

			var messageBusMock = new Mock<IMessageBus>();
			var httpContext = new DefaultHttpContext();

			httpContext.Request.Path = CqrsBusMiddlewareOptions.Default.EndpointUrl;
			httpContext.Request.Headers.Add("Message", "some_message");

			// When 
			await sut.InvokeAsync(httpContext, messageBusMock.Object);

			// Then
			messageBusMock.Verify(x => x.Execute("some_message", It.IsAny<Stream>()), Times.Once);
			httpContext.Response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
		}

		[Fact]
		public async void Test()
		{
			HttpContext httpContext = new DefaultHttpContext();
			httpContext.Response.Body = new MemoryStream();

			await httpContext.Response.WriteAsync("test");

			httpContext.Response.Body.ReadAsString().ShouldBe("test");
		}
	}
}
