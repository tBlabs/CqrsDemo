using System;

namespace CqrsDemo.Test
{
    public class ThisAssemblyTypesProvider : IAssemblyTypesProvider
    {
        public Type[] Types => this.GetType().Assembly.GetTypes();
    }
}