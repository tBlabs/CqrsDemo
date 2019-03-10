using System;

namespace Core.Services
{
    public interface IAssemblyTypesProvider
    {
        Type[] Types { get; }
    }
}