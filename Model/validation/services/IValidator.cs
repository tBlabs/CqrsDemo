using System;

namespace Model.validation.services
{
    public interface IValidator
    {
        bool Validate(object type);
    }
}