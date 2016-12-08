using System.Threading.Tasks;
using CQRSalad.EventSourcing;

namespace CQRSalad.Infrastructure
{
    public interface IQueryBus
    {
        Task<TResult> QueryAsync<TResult>(IQueryFor<TResult> query, string senderId);
    }
}