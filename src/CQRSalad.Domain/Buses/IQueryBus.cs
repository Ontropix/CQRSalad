using System.Threading.Tasks;

namespace CQRSalad.Domain
{
    public interface IQueryBus
    {
        Task<TResult> QueryAsync<TResult>(IQueryFor<TResult> query, string senderId);
    }
}