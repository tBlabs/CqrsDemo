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
	public static class MessageExtension
	{
		public static string ToMessagePackage(this IMessage message)
		{
			var args = JsonConvert.SerializeObject(message);
			MessagePackage package = new MessagePackage();
			package.Name = message.GetType().Name;
			package.Args = args;
			//return "{ 'name': '" + message.GetType().Name + "', 'args': \"" + args + "\" }";
			return JsonConvert.SerializeObject(package);
		}
	}
	public class WebApiHostIntegrationTests : IClassFixture<WebApplicationFactory<WebApiHost.Startup>>
	{
		private readonly WebApplicationFactory<WebApiHost.Startup> _factory;

		public WebApiHostIntegrationTests(WebApplicationFactory<WebApiHost.Startup> factory)
		{
			this._factory = factory;
		}

		/*
		 *
		 *
		 */

		[Fact]
		public async Task PostTest()
		{
			var client = _factory.CreateClient();

			var message = new SampleQuery() { Foo = "Bar" };

			//var content = new StringContent("{ 'name': 'SampleQuery', 'args': \"{ 'Foo': 'Bar' }\" }", Encoding.UTF8, "application/json");

			string messageAsJson = message.ToMessagePackage();
			var content = new StringContent(messageAsJson, Encoding.UTF8, "application/json");

			var response = await client.PostAsync("/", content);

			response.EnsureSuccessStatusCode();
			var responseContent = await response.Content.ReadAsStringAsync();

		///	responseContent.ShouldBe("Bar");

			var res = JsonConvert.DeserializeObject<AppResponse<SampleQueryResponse>>(responseContent);
			res.IsException.ShouldBeFalse();
			res.Response.Baz.ShouldBe("Bar");
		}
	}


}
