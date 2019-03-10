using System.Threading.Tasks;

namespace Core.Cqrs
{
    public interface ICommandHandler<T> where T : ICommand
    {
        Task Handle(T command);
    }
}
