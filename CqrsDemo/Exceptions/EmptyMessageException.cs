using System;

namespace Core.Exceptions
{
	public class EmptyMessageException : Exception
	{
		public EmptyMessageException() : base("Message can not be empty")
		{ }
	}
}