using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Cqrs
{
	public class AppResponse<T>
	{
		public bool IsException { get; set; }
		public T Response { get; set; }
	}
}
