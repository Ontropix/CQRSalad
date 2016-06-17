using System.Threading.Tasks;
using CQRSalad.Dispatching.NEW.Descriptors;

namespace CQRSalad.Dispatching.NEW.Context
{
    internal class DispatcherContextExecutor
    {
        private readonly HandlerActionDescriptor _actionDescriptor;
        private readonly HandlerExecutor _executor;

        public DispatcherContextExecutor(HandlerActionDescriptor actionDescriptor, HandlerExecutor executor)
        {
            _actionDescriptor = actionDescriptor;
            _executor = executor;
        }

        internal async Task Execute(DispatchingContext context)
        {
            if (_actionDescriptor.HandlerAction.IsAsync())
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
