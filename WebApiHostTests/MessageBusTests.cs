using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using ModuleA;
using ModuleB;
using Newtonsoft.Json;
using Shouldly;
using tBlabs.Cqrs.Core.Interfaces;
using tBlabs.Cqrs.Middleware;
using WebApiHost.Tests.Helpers;
using Xunit;

namespace WebApiHost.Tests
{
    public class WebApiHostIntegrationTests : IClassFixture<WebApplicationFactory<WebApiHost.Startup>>
    {
        private readonly HttpClient client;

        public WebApiHostIntegrationTests(WebApplicationFactory<WebApiHost.Startup> factory)
        {
            client = factory.CreateClient();
        }

        #region Helpers

        private async Task<(HttpStatusCode, TResponse)> SendMessage<TResponse>(IMessage message)
        {
            string messageAsJson = message.ToJson();
            var content = new StringContent(messageAsJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(CqrsBusMiddlewareOptions.Default.EndpointUrl, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var responseObject = JsonConvert.DeserializeObject<TResponse>(responseContent);

                return (response.StatusCode, responseObject);
            }

            return (response.StatusCode, (TResponse)(object)responseContent);
        }

        private async Task<HttpStatusCode> SendCommand(ICommand command)
        {
            var (http, response) = await SendMessage<object>(command);
            response.ShouldBeNull();
            return http;
        }

        private async Task<(HttpStatusCode, TResponse)> PostMessage<TResponse>(IMessage message, Stream stream)
        {
            string messageAsJson = message.ToJson();
            var content = new StreamContent(stream);
            content.Headers.Add(CqrsBusMiddlewareOptions.Default.MessageHeader, messageAsJson);

            var response = await client.PostAsync(CqrsBusMiddlewareOptions.Default.EndpointUrl, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var responseObject = JsonConvert.DeserializeObject<TResponse>(responseContent);

                return (response.StatusCode, responseObject);
            }

            return (response.StatusCode, (TResponse)(object)responseContent);
        }

        #endregion

        [Fact]
        public async Task Should_execute_handler_from_external_module()
        {
            var query = new DoubleValueQuery { Value = 5 };

            var (httpStatus, response) = await SendMessage<int>(query);
            
            httpStatus.ShouldBe(HttpStatusCode.OK);
            response.ShouldBe(10);
        }

        [Fact]
        public async Task Command_should_be_executed()
        {
            var message = new Command { Value = 3 };

            var httpStatus = await SendCommand(message);

            httpStatus.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Query_should_be_executed()
        {
            var message = new Query { Value = 2 };

            var (httpStatus, response) = await SendMessage<int>(message);

            httpStatus.ShouldBe(HttpStatusCode.OK);
            response.ShouldBe(4);
        }

        [Fact]
        public async Task Command_with_stream_should_be_executed()
        {
            var message = new CommandWithStream { Foo = "bar" };
            var stream = new MemoryStream();

            var (httpStatus, response) = await PostMessage<object>(message, stream);

            httpStatus.ShouldBe(HttpStatusCode.OK);
            response.ShouldBeNull();
        }

        [Fact]
        public async Task Not_existing_message_should_return_404()
        {
            var message = new NotRegisteredMessage();

            var (httpStatus, response) = await SendMessage<string>(message);

            httpStatus.ShouldBe(HttpStatusCode.NotFound);
            response.ShouldContain("not found");
        }

        [Fact]
        public async Task Handler_exception_should_be_catch()
        {
            var message = new Query { Value = 0 }; // Value==0 make handler to throw

            var (httpStatus, response) = await SendMessage<string>(message);

            httpStatus.ShouldBe(HttpStatusCode.InternalServerError);
            response.ShouldContain("SomeExceptionMessage");
        }        
        
        [Fact]
        public async Task Handler_should_throw_with_non_standard_HttpStatusCode()
        {
            var message = new Query { Value = (-1) }; // Value==-1 make handler to throw special exception

            var (httpStatus, response) = await SendMessage<string>(message);

            httpStatus.ShouldBe((HttpStatusCode)444);
        }
    }
}
