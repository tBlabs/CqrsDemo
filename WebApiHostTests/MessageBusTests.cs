using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Services;
using Core.Test;
using Messages.Commands;
using Messages.Dto;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Newtonsoft.Json;
using Shouldly;
using WebApiHost;
using WebApiHostTests.Helpers;
using Xunit;

namespace WebApiHostTests
{
	public class WebApiHostIntegrationTests : IClassFixture<WebApplicationFactory<WebApiHost.Startup>>
	{
		public class Command : ICommand
		{
			public int Value { get; set; }
		}

		public class Query : IQuery<int>
		{
			public int Value { get; set; }
		}

		public class CommandWithStream : ICommandWithStream
		{
			public Stream Stream { get; set; }
			public string Foo { get; set; }
		}

		public class CommandWithStreamHandler : ICommandHandler<CommandWithStream>
		{
			public Task Handle(CommandWithStream command)
			{
				return Task.CompletedTask;
			}
		}

		public class CommandHandler : ICommandHandler<Command>
		{
			public Task Handle(Command command)
			{
				return Task.CompletedTask;
			}
		}

		public class QueryHandler : IQueryHandler<Query, Task<int>>
		{
			public Task<int> Handle(Query query)
			{
				if (query.Value == 0)
				{
					throw new Exception("SomeExceptionMessage");
				}

				return Task.FromResult(query.Value * 2);
			}
		}

		public class NotRegisteredMessage : IMessage
		{ }

		private readonly HttpClient client;

		public WebApiHostIntegrationTests(WebApplicationFactory<WebApiHost.Startup> factory)
		{
			//_factory = factory;
			client = factory.CreateClient();
			//var typesProviderMock = new Mock<ITypesProvider>();
			//typesProviderMock.Setup(x => x.Types).Returns(new Type[]
			//{
			//	typeof(Command), typeof(Query),
			//	typeof(CommandHandler), typeof(QueryHandler),
			//	typeof(CommandWithStream), typeof(CommandWithStreamHandler)
			//});

			//client = factory.WithWebHostBuilder(builder =>
			//		builder.ConfigureServices(services =>
			//			services.AddSingleton<ITypesProvider>(typesProviderMock.Object)))
			//	.CreateClient();
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
		public async Task Command_should_be_executed()
		{
			var message = new Command { Value = 3 };

			var (httpStatus, response) = await PostMessage<object>(message);

			httpStatus.ShouldBe(HttpStatusCode.OK);
			response.ShouldBeNull();
		}

		[Fact]
		public async Task Query_should_be_executed()
		{
			var message = new Query { Value = 2 };

			var (httpStatus, response) = await PostMessage<int>(message);

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

			var (httpStatus, response) = await PostMessage<string>(message);

			httpStatus.ShouldBe(HttpStatusCode.NotFound);
			response.ShouldContain("not found");
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
