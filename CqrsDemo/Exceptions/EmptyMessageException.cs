using System;

namespace tBlabs.Cqrs.Core.Exceptions
{
	public class EmptyMessageException : Exception
	{
		public EmptyMessageException() : base("Message can not be empty")
		{ }
	}
}