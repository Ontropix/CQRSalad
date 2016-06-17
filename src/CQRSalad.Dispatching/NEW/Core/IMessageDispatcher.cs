using System.Threading.Tasks;

namespace CQRSalad.Dispatching.NEW.Core
{
    public interface IMessageDispatcher
    {
        Task<object> SendAsync(object message);
        Task PublishAsync<TMessage>(TMessage message);
    }
}