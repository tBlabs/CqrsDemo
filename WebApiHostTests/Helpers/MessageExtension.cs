using System.Collections.Generic;
using Newtonsoft.Json;
using tBlabs.Cqrs.Core.Interfaces;

namespace WebApiHost.Tests.Helpers
{
	public static class MessageExtension
	{
		public static string ToJson(this IMessage message)
		{
			var messageName = message.GetType().Name;
			var args = message; 
			var dict = new Dictionary<string, object>() { { messageName, args } };

			return JsonConvert.SerializeObject(dict);
		}
	}
}