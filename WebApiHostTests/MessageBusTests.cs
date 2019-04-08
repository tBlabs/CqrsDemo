using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Test;
using Messages.Commands;
using Messages.Dto;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Shouldly;
using WebApiHost;
using WebApiHostTests.Helpers;
using Xunit;

namespace WebApiHostTests
{
	public class WebApiHostIntegrationTests : IClassFixture<WebApplicationFactory<WebApiHost.Startup>>
	{
		//private readonly WebApplicationFactory<WebApiHost.Startup> _factory;
		private readonly HttpClient client;

		public WebApiHostIntegrationTests(WebApplicationFactory<WebApiHost.Startup> factory)
		{
			//_factory = factory;
			client = factory.CreateClient();
		}

		#region Helpers

		private async Task<(HttpStatusCode, T)> PostMessage<T>(IMessage message)
		{
			string messageAsJson = message.ToJson();
			var content = new StringContent(messageAsJson, Encoding.UTF8, "application/json");
			//content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			var response = await client.PostAsync("/CqrsBus", content);
			var responseContent = await response.Content.ReadAsStringAsync();

			if (response.IsSuccessStatusCode)
			{
				var responseObject = JsonConvert.DeserializeObject<T>(responseContent);

				return (response.StatusCode, responseObject);
			}
			else
			{
				return (response.StatusCode, (T)(object)responseContent);
			}
		}

		private async Task<(HttpStatusCode, T)> PostMessage<T>(IMessage message, Stream stream)
		{
			string messageAsJson = message.ToJson();
			var content = new StreamContent(stream);
			content.Headers.Add("Message", messageAsJson);

			var response = await client.PostAsync("/CqrsBus", content);
			var responseContent = await response.Content.ReadAsStringAsync();

			if (response.IsSuccessStatusCode)
			{
				var responseObject = JsonConvert.DeserializeObject<T>(responseContent);

				return (response.StatusCode, responseObject);
			}
			else
			{
				return (response.StatusCode, (T)(object)responseContent);
			}
		}

		#endregion

		[Fact]
		public async Task Query_from_this_project_should_be_executed()
		{
			var message = new Query { Value = 5 };

			var (httpStatus, response) = await PostMessage<int>(message);

			httpStatus.ShouldBe(HttpStatusCode.OK);
			response.ShouldBe(10);
		}

		[Fact]
		public async Task Stream_()
		{
			var message = new CommandWithStream { Foo = "bar" };
			var stream = new MemoryStream();

			var (httpStatus, response) = await PostMessage<object>(message, stream);

			httpStatus.ShouldBe(HttpStatusCode.OK);
			response.ShouldBeNull();
		}

		[Fact]
		public async Task Command_from_another_project_should_be_executed()
		{
			var message = new Command { Value = 3 };

			var (httpStatus, response) = await PostMessage<object>(message);

			httpStatus.ShouldBe(HttpStatusCode.OK);
			response.ShouldBeNull();
		}

		[Fact]
		public async Task Query_from_another_project_should_be_executed()
		{
			var message = new Query { Value = 2 };

			var (httpStatus, response) = await PostMessage<int>(message);

			httpStatus.ShouldBe(HttpStatusCode.OK);
			response.ShouldBe(4);
		}

		[Fact]
		public async Task Not_existing_message_should_return_404()
		{
			var message = new NotRegisteredMessage();

			var (httpStatus, response) = await PostMessage<string>(message);

			httpStatus.ShouldBe(HttpStatusCode.NotFound);
			response.ShouldBe("Message '" + nameof(NotRegisteredMessage) + "' not found");
		}

		[Fact]
		public async Task Handler_exception_should_be_catch()
		{
			var message = new Query { Value = 0 }; // Value==0 will make handler to throw exception

			var (httpStatus, response) = await PostMessage<string>(message);

			httpStatus.ShouldBe(HttpStatusCode.InternalServerError);
			response.ShouldBe("SomeExceptionMessage");
		}
	}
}
