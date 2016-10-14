using System.Threading.Tasks;
using Kutcha.Core;

namespace CQRSalad.Infrastructure.Logging
{
    public interface IDispatcherContextLog 
    {
        string Id { get; set; }
    }

    public interface IDispatcherContextLogger
    {
        Task OpenMessageLog(object message, object handler);
        Task LogResult(object result);
        Task LogError(string errorMessage);

        Task ReadLogById(string logId);
        Task ReadLogs(int from = 0, int count = 0);
        Task ReadLogsWithError(int from = 0, int count = 0);
    }

    public class KutchaDispatcherContextLog : IDispatcherContextLog, IKutchaRoot
    {
        string IKutchaRoot.Id => this.Id;
        public string Id { get; set; }
    }

    public class KutchaDispatcherContextLogger : IDispatcherContextLogger
    {
        private readonly IKutchaStore<KutchaDispatcherContextLog> _store;

        public KutchaDispatcherContextLogger(IKutchaStore<KutchaDispatcherContextLog> store)
        {
            _store = store;
        }

        public Task OpenMessageLog(object message, object handler)
        {
            throw new System.NotImplementedException();
        }

        public Task LogResult(object result)
        {
            throw new System.NotImplementedException();
        }

        public Task LogError(string errorMessage)
        {
            throw new System.NotImplementedException();
        }

        public Task ReadLogById(string logId)
        {
            throw new System.NotImplementedException();
        }

        public Task ReadLogs(int @from = 0, int count = 0)
        {
            throw new System.NotImplementedException();
        }

        public Task ReadLogsWithError(int @from = 0, int count = 0)
        {
            throw new System.NotImplementedException();
        }
    }
}
