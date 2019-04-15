using System;

namespace Core.Exceptions
{
	public class NotFoundException : Exception
	{
		public NotFoundException(string modelName) : base(modelName)
		{ }
	}
}