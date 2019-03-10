using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Core.Cqrs;
using Messages.Dto;
using Messages.Queries;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace WebApiHostTests
{
	public class SampleMsg : IQuery<int>
	{
		public int Value { get; set; }
	}

	public class SampleHandler : IQueryHandler<SampleMsg, Task<int>>
	{
		public Task<int> Handle(SampleMsg query)
		{
			return Task.FromResult(query.Value * 2);
		}
	}

	public class WebApiHostIntegrationTests : IClassFixture<WebApplicationFactory<WebApiHost.Startup>>
	{
		private readonly WebApplicationFactory<WebApiHost.Startup> _factory;
		private readonly HttpClient client;

		public WebApiHostIntegrationTests(WebApplicationFactory<WebApiHost.Startup> factory)
		{
			_factory = factory;
			client = _factory.CreateClient();
		}

		[Fact]
		public async Task LocalTest()
		{
			var message = new SampleMsg { Value = 2 };

			string messageAsJson = message.ToJson();
			var content = new StringContent(messageAsJson, Encoding.UTF8, "application/json");

			var response = await client.PostAsync("/", content);

			response.EnsureSuccessStatusCode();
			var responseContent = await response.Content.ReadAsStringAsync();

			var res = JsonConvert.DeserializeObject<int>(responseContent);
			res.ShouldBe(4);
		}

		[Fact]
		public async Task Valid_query_should_pass()
		{
			var message = new SampleQuery() { Foo = "Bar" };

			string messageAsJson = message.ToJson();
			var content = new StringContent(messageAsJson, Encoding.UTF8, "application/json");

			var response = await client.PostAsync("/", content);

			response.EnsureSuccessStatusCode();
			var responseContent = await response.Content.ReadAsStringAsync();

			var res = JsonConvert.DeserializeObject<SampleQueryResponse>(responseContent);
			res.Baz.ShouldBe("Bar");
		}

		[Fact]
		public async Task Handler_exception_should_be_catch()
		{
			var message = new SampleQuery() { Foo = "Ex" };

			string messageAsJson = message.ToJson();
			var content = new StringContent(messageAsJson, Encoding.UTF8, "application/json");

			var response = await client.PostAsync("/", content);

			response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);

			var responseContent = await response.Content.ReadAsStringAsync();

			responseContent.ShouldBe("Exception");
		}
	}
}
