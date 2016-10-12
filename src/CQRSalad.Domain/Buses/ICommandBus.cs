using System.Threading.Tasks;

namespace CQRSalad.Domain
{
    public class CommandResult
    {
        public CommandStatus Status { get; set; }
        public string ErrorMessage { get; set; }
    }

    public enum CommandStatus
    {
        Ok = 200,
        Error = 500
    }

    public interface ICommandBus
    {
        /// <summary>
        /// Sends command.
        /// </summary>
        Task SendAsync<TCommand>(TCommand command, string senderId) where TCommand : class;
    }
}
