namespace Messages.validation.services
{
    public interface IValidator
    {
        bool Validate(object type);
    }
}