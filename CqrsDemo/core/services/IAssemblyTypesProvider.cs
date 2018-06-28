using System;

namespace CqrsDemo
{
    public interface IAssemblyTypesProvider
    {
        Type[] Types { get; }
    }
}