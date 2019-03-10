namespace Core.Cqrs
{
    public interface IQueryHandler<in TQuery, out TResult> where TQuery : IQueryBase
    {
        TResult Handle(TQuery query);
    }
}
