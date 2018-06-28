using System;
using System.Reflection;

namespace CqrsDemo
{
    public class AssemblyTypesProvider : IAssemblyTypesProvider
    {
        public Type[] Types => Assembly.GetExecutingAssembly().GetTypes();
    }
}