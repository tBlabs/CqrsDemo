using System;

namespace Core.Exceptions
{
	public class HandlerNotFoundException : Exception
	{
		public HandlerNotFoundException(Type messageType) : base($"Handler for message '{messageType.Name}' not found")
		{ }
	}
}