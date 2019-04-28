using System.Threading.Tasks;

namespace tBlabs.Cqrs.Core.Interfaces
{
    public interface ICommandHandler<T> where T : ICommand
    {
        Task Handle(T command);
    }
}
