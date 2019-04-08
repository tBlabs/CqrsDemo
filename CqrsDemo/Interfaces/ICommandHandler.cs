using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ICommandHandler<T> where T : ICommand
    {
        Task Handle(T command);
    }
}
