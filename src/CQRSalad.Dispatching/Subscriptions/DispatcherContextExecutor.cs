using System;
using System.Threading.Tasks;

namespace CQRSalad.Dispatching
{
    internal class DispatcherContextExecutor
    {
        private readonly MessageInvoker _executor;

        public DispatcherContextExecutor(MessageInvoker executor)
        {
            _executor = executor;
        }

        public async Task Execute(DispatchingContext context)
        {
            Task taskResult = (Task)_executor(context.HandlerInstance, context.MessageInstance);
            await taskResult;
            context.Result = taskResult.GetTaskResult();

            //context.Result = _executor(context.HandlerInstance, context.MessageInstance);
            //await Task.CompletedTask;
        }

        //public Task HandleResponse(object response, Func<object, Task> callback, Func<Exception, Task> errorCallback)
        //{
            //try
            //{
            //    var taskResponse = response as Task;
            //    if (taskResponse != null)
            //    {
            //        if (taskResponse.Status == TaskStatus.Created)
            //        {
            //            taskResponse.Start();
            //        }

            //        return taskResponse
            //            .Continue(task =>
            //            {
            //                if (task.IsFaulted)
            //                    return errorCallback(task.Exception.UnwrapIfSingleException());

            //                if (task.IsCanceled)
            //                    return errorCallback(new OperationCanceledException("The async Task operation was cancelled"));

            //                if (task.IsCompleted)
            //                {
            //                    var taskResult = task.GetResult();

            //                    var taskResults = taskResult as Task[];

            //                    if (taskResults == null)
            //                    {
            //                        var subTask = taskResult as Task;
            //                        if (subTask != null)
            //                            taskResult = subTask.GetResult();

            //                        return callback(taskResult);
            //                    }

            //                    if (taskResults.Length == 0)
            //                        return callback(TypeConstants.EmptyObjectArray);

            //                    var firstResponse = taskResults[0].GetResult();
            //                    var batchedResponses = firstResponse != null
            //                        ? (object[])Array.CreateInstance(firstResponse.GetType(), taskResults.Length)
            //                        : new object[taskResults.Length];
            //                    batchedResponses[0] = firstResponse;
            //                    for (var i = 1; i < taskResults.Length; i++)
            //                    {
            //                        batchedResponses[i] = taskResults[i].GetResult();
            //                    }
            //                    return callback(batchedResponses);
            //                }

            //                return errorCallback(new InvalidOperationException("Unknown Task state"));
            //            });
            //    }

            //    return callback(response);
            //}
            //catch (Exception ex)
            //{
            //    return errorCallback(ex);
            //}
        //}
    }
}