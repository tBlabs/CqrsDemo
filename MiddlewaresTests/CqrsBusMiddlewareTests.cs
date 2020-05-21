using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json;
using Shouldly;
using tBlabs.Cqrs.Core.Exceptions;
using tBlabs.Cqrs.Core.Services;
using tBlabs.Cqrs.Middleware.Extensions;
using Xunit;

namespace tBlabs.Cqrs.Middleware.Tests
{
    public class CqrsBusMiddlewareTests
    {
        [Fact]
        public void New_options_should_inherit_from_default()
        {
            var options = new CqrsBusMiddlewareOptions() { EndpointUrl = "abc" };
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
            (await httpContext.Response.Body.ReadAsString()).ShouldBe("\"value\"");
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
            (await httpContext.Response.Body.ReadAsString()).ShouldBe("Message 'some_message' not found");
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
            (await httpContext.Response.Body.ReadAsString()).ShouldBe("some exception message");
        }

        [Fact]
        public async void Stream_should_be_returned_for_query_returning_stream()
        {
            // Given
            var nextMock = new Mock<RequestDelegate>();

            var messageBusMock = new Mock<IMessageBus>();
            messageBusMock.Setup(x => x.Execute(It.IsAny<string>(), null))
                .Returns(Task.FromResult((object)new MemoryStream(Encoding.ASCII.GetBytes("StreamData1"))));

            var sut = new CqrsBusMiddleware(nextMock.Object, CqrsBusMiddlewareOptions.Default);

            var httpContext = new DefaultHttpContext();
            httpContext.Response.Body = new MemoryStream();
            httpContext.Request.Path = CqrsBusMiddlewareOptions.Default.EndpointUrl;

            // When
            await sut.InvokeAsync(httpContext, messageBusMock.Object);

            // Then
            httpContext.Response.StatusCode.ShouldBe((int)HttpStatusCode.OK);

            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            (await httpContext.Response.Body.ReadAsString()).ShouldBe("StreamData1");
        }

        [Fact]
        public async void Next_middleware_should_be_called_if_path_was_different_than_selected()
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
            (await httpContext.Response.Body.ReadAsString()).ShouldBe("test");
        }

        [Fact]
        public async void Http_500_with_stack_trace_should_be_returned_at_handler_exception()
        {
            // Given
            var middlewareOptions = new CqrsBusMiddlewareOptions
            {
                AddStackTrace = true
            };
            var sut = new CqrsBusMiddleware(null, middlewareOptions);

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
            var fullExceptionAsJson = await httpContext.Response.Body.ReadAsString();
            //// dynamic exception = JsonConvert.DeserializeObject(fullExceptionAsJson);
            //// ((string)exception.ClassName).ShouldBe("System.Exception");
            //// ((string)exception.Message).ShouldBe("some exception message");
            //// ((string)exception.Source).ShouldBe("Moq");
            fullExceptionAsJson.ShouldContain("some exception message");
        }
    }
}
