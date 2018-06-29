using System;
using System.Collections.Generic;
using System.Text;
using Core;
using FluentValidation;

namespace Model.validation.services
{
    public class MyValidator : IMyValidator
    {
        public Dictionary<Type, Type> Validators { get; } = new Dictionary<Type, Type>();

        public MyValidator(IAssemblyTypesProvider assemblyTypes)
        {
            foreach (var t in assemblyTypes.Types)
            {
                if (t.IsClass && t.IsPublic && !t.IsAbstract
                    && t.IsSubclassOf(typeof(AbstractValidator<>)))
                {
                    var abstractValidator = t.BaseType;
                    var abstractValidatorEntity = abstractValidator.GetGenericArguments()[0];

                    Validators.Add(abstractValidatorEntity, t);
                }
            }
        }

        private Type GetMessageValidator(Type entity)
        {
            return Validators[entity];
        }

        public bool Validate(object type)
        {
            var validatorType = GetMessageValidator(type.GetType());




            return false;
        }
    }
}
