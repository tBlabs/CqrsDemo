namespace Core.Cqrs
{
    public interface IQuery<out TResult> : IQueryBase
    { }
}
