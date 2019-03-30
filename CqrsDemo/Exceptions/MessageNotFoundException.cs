using System;

namespace Core.Exceptions
{
	public class MessageNotFoundException : Exception
	{
		public MessageNotFoundException(string name) : base($"Message {name} not found")
		{ }
	}
}