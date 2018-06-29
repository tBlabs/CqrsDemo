using System;

namespace Core.Test
{
    public class ThisAssemblyTypesProvider : IAssemblyTypesProvider
    {
        public Type[] Types => this.GetType().Assembly.GetTypes();
    }
}