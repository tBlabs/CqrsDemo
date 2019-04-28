using System;

namespace tBlabs.Cqrs.Core.Exceptions
{
	public class NotFoundException : Exception
	{
		public NotFoundException(string modelName) : base(modelName)
		{ }
	}
}