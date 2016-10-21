using System.Threading.Tasks;

namespace CQRSalad.Domain
{
    public interface ICommandBus
    {
        /// <summary>
        /// Sends command.
        /// </summary>
        Task CommandAsync<TCommand>(TCommand command, string senderId) where TCommand : class, ICommand;
    }
}
