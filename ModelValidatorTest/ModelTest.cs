using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Model;
using Xunit;
using System.Configuration;
using Core.Test;
using Model.validation.services;

namespace ModelTest
{
    public class ModelTest
    {
        [Fact]
        public void Test()
        {
            var validator = new EntityValidator();
            var validationResult = validator.Validate(new Entity() { StringProp = "foobar" });

            validationResult.IsValid.Should().Be(true);
        }

        [Fact]
        public void ValidationProvider_should_find_validators()
        {
            var thisProjectTypesProvider = new ThisAssemblyTypesProvider();
            var validator = new MyValidator(thisProjectTypesProvider);

            var entity = new Entity();

            validator.Validate(entity).Should().BeTrue();
        }
    }
}
