using System;

namespace Core
{
    public interface IAssemblyTypesProvider
    {
        Type[] Types { get; }
    }
}