using System;
using System.Collections.Generic;

namespace Core.Services
{
	public class SolutionTypesProvider : ISolutionTypesProvider
	{
		public Type[] Types
		{
			get
			{
				var types = new List<Type>();

				foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
				{
					types.AddRange(assembly.GetTypes());
				}

				return types.ToArray();
			}
		}
    }
}