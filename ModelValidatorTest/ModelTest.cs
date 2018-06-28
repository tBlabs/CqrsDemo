using FluentAssertions;
using Model;
using Xunit;
using System.Configuration;

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
    }
}
