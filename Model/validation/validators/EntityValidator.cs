using FluentValidation;


namespace Model
{
    public class EntityValidator : AbstractValidator<Entity>
    {
        public EntityValidator()
        {
            RuleFor(e => e.StringProp).NotEmpty().Must(s => s.StartsWith("foo"));
        }
    }
}
