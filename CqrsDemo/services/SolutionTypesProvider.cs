using System;
using System.Collections.Generic;

namespace tBlabs.Cqrs.Core.Services
{
	public class SolutionTypesProvider : ITypesProvider
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