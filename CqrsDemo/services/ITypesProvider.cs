using System;

namespace tBlabs.Cqrs.Core.Services
{
    public interface ITypesProvider
    {
        Type[] Types { get; }
    }
}