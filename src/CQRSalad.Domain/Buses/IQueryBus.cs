using System.Threading.Tasks;

namespace CQRSalad.Domain
{
    public class QueryResult<TResult>
    {
        public TResult Result { get; set; }
        public QueryStatus Status { get; set; }
        public string ErrorMessage { get; set; }
    }

    public enum QueryStatus
    {
        Ok = 200,
        Error = 500
    }

    public interface IQueryBus
    {
        Task<QueryResult<TResult>> QueryAsync<TResult>(IQuery<TResult> query, string senderId);
    }
}