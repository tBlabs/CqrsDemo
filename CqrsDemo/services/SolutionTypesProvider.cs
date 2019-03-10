using System;
using System.Collections.Generic;

namespace Core.Services
{
	public class SolutionTypesProvider : IAssemblyTypesProvider
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



				//foreach (var library in DependencyContext.Default.RuntimeLibraries)
				//{
				//	foreach (var libraryDependency in library.Dependencies)
				//	{
				//		if (libraryDependency.Name.Contains("Messages"))
				//			types.AddRange(libraryDependency.GetType().Assembly.GetTypes().Where(x=>x.IsClass && x.IsPublic && !x.IsAbstract));
				//	}
				//}
				//types.AddRange(
				//	AssemblyLoadContext.Default.LoadFromAssemblyPath(Directory.GetCurrentDirectory()).GetTypes()
				//);

				return types.ToArray();
				//var referencedAssemblies = this.GetType().Assembly
				//	.GetReferencedAssemblies();

				//return referencedAssemblies.Select(Assembly.Load)
				//	.SelectMany(x => x.DefinedTypes)
				//	.Select(x => x.AsType())
				//	.ToArray();
				//	return this.GetType().Assembly.GetTypes();
				//return Assembly.GetExecutingAssembly().GetTypes();
				//return this.GetType().Assembly.GetTypes();
			}
		}

	//  public Type[] TTT => this.GetType().Assembly.GetTypes();
    }
}