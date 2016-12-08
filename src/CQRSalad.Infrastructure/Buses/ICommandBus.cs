using System.Threading.Tasks;
using CQRSalad.EventSourcing;

namespace CQRSalad.Infrastructure
{
    public interface ICommandBus
    {
        /// <summary>
        /// Sends command.
        /// </summary>
        Task CommandAsync<TCommand>(TCommand command, string senderId) where TCommand : class, ICommand;
    }
}
