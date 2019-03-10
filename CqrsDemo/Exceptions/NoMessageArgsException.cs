using System;

namespace Core.Exceptions
{
	public class NoMessageArgsException : Exception
	{
		public NoMessageArgsException() : base("No message package args detected. Should be at least an empty string.")
		{ }
	}
}