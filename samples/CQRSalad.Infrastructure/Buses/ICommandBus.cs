using System.Threading.Tasks;
using CQRSalad.Domain;

namespace CQRSalad.Infrastructure.Buses
{
    public interface ICommandBus
    {
        /// <summary>
        /// Sends command.
        /// </summary>
        Task CommandAsync<TCommand>(TCommand command, string senderId) where TCommand : class, ICommand;
    }
}
