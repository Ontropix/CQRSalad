using System.Threading.Tasks;

namespace CQRSalad.Dispatching.Context
{
    internal interface IDispatcherContextExecutor
    {
        Task Execute(DispatchingContext context);
    }

    internal class SyncContextExecutor : IDispatcherContextExecutor
    {
        private readonly HandlerExecutor _executor;

        public SyncContextExecutor(HandlerExecutor executor)
        {
            _executor = executor;
        }

        public async Task Execute(DispatchingContext context)
        {
            context.Result = _executor(context.HandlerInstance, context.MessageInstance);
            await Task.CompletedTask;
        }
    }

    internal class AsyncContextExecutor : IDispatcherContextExecutor
    {
        private readonly HandlerExecutor _executor;

        public AsyncContextExecutor(HandlerExecutor executor)
        {
            _executor = executor;
        }

        public async Task Execute(DispatchingContext context)
        {
            Task taskResult = (Task)_executor(context.HandlerInstance, context.MessageInstance);
            await taskResult;
            context.Result = taskResult.GetTaskResult();
        }
    }
}
