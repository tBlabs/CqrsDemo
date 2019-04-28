using System;

namespace tBlabs.Cqrs.Core.Exceptions
{
	public class HandlerNotFoundException : NotFoundException
	{
		public HandlerNotFoundException(Type messageType) : base($"Handler for message '{messageType.Name}' not found")
		{ }
	}
}