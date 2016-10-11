using System.Threading.Tasks;

namespace CQRSalad.Dispatching.Core
{
    public interface IMessageDispatcher
    {
        Task<object> SendAsync(object message);
        Task PublishAsync<TMessage>(TMessage message);
    }
}