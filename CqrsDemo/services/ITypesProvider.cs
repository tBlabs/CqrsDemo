using System;

namespace Core.Services
{
    public interface ITypesProvider
    {
        Type[] Types { get; }
    }
}