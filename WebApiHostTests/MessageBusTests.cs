using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Cqrs;
using Messages.Dto;
using Messages.Queries;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace WebApiHostTests
{
	public class SampleMsg : IQuery<int>
	{

	}

	public class SampleHandler : IQueryHandler<SampleMsg, int>
	{
		public int Handle(SampleMsg query)
		{
			return 5;
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
			var message = new SampleMsg();

			string messageAsJson = message.ToJson();
			var content = new StringContent(messageAsJson, Encoding.UTF8, "application/json");

			var response = await client.PostAsync("/", content);

			response.EnsureSuccessStatusCode();
			var responseContent = await response.Content.ReadAsStringAsync();

			var res = JsonConvert.DeserializeObject<AppResponse<int>>(responseContent);
			res.IsException.ShouldBeFalse();
			res.Response.ShouldBe(5);
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

			var res = JsonConvert.DeserializeObject<AppResponse<SampleQueryResponse>>(responseContent);
			res.IsException.ShouldBeFalse();
			res.Response.Baz.ShouldBe("Bar");
		}

		[Fact]
		public async Task Handler_exception_should_be_catch()
		{
			var message = new SampleQuery() { Foo = "Ex" };

			string messageAsJson = message.ToJson();
			var content = new StringContent(messageAsJson, Encoding.UTF8, "application/json");

			var response = await client.PostAsync("/", content);

			response.EnsureSuccessStatusCode();

			var responseContent = await response.Content.ReadAsStringAsync();

			var res = JsonConvert.DeserializeObject<AppResponse<string>>(responseContent);
			res.IsException.ShouldBeTrue();
			res.Response.ShouldBe("Exception");
		}
	}
}
