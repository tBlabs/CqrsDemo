using System;

namespace Model.validation.services
{
    public interface IMyValidator
    {
        bool Validate(object type);
    }
}