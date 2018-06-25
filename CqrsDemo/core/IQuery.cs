namespace CqrsDemo
{
    public interface IQuery<out TResult> : IQueryBase
    { }
}
