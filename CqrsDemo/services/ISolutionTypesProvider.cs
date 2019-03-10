using System;

namespace Core.Services
{
    public interface ISolutionTypesProvider
    {
        Type[] Types { get; }
    }
}