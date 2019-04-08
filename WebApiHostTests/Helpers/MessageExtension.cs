using System.Collections.Generic;
using Core.Interfaces;
using Newtonsoft.Json;

namespace WebApiHostTests.Helpers
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