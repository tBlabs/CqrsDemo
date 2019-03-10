using Core.Cqrs;
using Newtonsoft.Json;

namespace WebApiHostTests
{
	public static class MessageExtension
	{
		public static string ToJson(this IMessage message)
		{
			var args = JsonConvert.SerializeObject(message);
			MessagePackage package = new MessagePackage();
			package.Name = message.GetType().Name;
			package.Args = args;
			//return "{ 'name': '" + message.GetType().Name + "', 'args': \"" + args + "\" }";
			return JsonConvert.SerializeObject(package);
		}
	}
}