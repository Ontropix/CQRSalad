using System.Threading.Tasks;

namespace CQRSalad.Dispatching.Context
{
    internal class DispatcherContextExecutor
    {
        private readonly HandlerExecutor _executor;
        private readonly bool _isAsync;

        public DispatcherContextExecutor(HandlerExecutor executor, bool isAsync)
        {
            _executor = executor;
            _isAsync = isAsync;
        }

        internal async Task Execute(DispatchingContext context)
        {
            if (_isAsync)
            {
                Task taskResult = (Task)_executor(context.HandlerInstance, context.MessageInstance);
                await taskResult;
                context.Result = taskResult.GetTaskResult();
            }
            else
            {
                context.Result = _executor(context.HandlerInstance, context.MessageInstance);
            }
        }
    }
}
