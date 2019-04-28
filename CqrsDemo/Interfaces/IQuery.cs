namespace tBlabs.Cqrs.Core.Interfaces
{
    public interface IQuery<out TResult> : IQueryBase
    { }
}
