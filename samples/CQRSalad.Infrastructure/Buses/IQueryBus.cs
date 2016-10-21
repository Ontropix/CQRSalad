using System.Threading.Tasks;
using CQRSalad.Domain;

namespace CQRSalad.Infrastructure.Buses
{
    public interface IQueryBus
    {
        Task<TResult> QueryAsync<TResult>(IQueryFor<TResult> query, string senderId);
    }
}