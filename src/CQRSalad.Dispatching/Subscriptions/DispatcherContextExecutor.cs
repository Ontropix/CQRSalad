using System;
using System.Reflection;
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
            var result = _executor(context.HandlerInstance, context.MessageInstance);

            var taskResult = result as Task;
            if (taskResult == null)
            {
                context.Result = result;
                return;
            }

            if (taskResult.Status == TaskStatus.Created)
            {
                taskResult.Start();
            }

            //if (taskResult.IsFaulted)
            //{
            //    return errorCallback(task.Exception.UnwrapIfSingleException());
            //}

            //if (taskResult.IsCanceled)
            //{
            //    return errorCallback(new OperationCanceledException("The async Task operation was cancelled"));
            //}

            if (!taskResult.IsCompleted)
            {
                //return errorCallback(new InvalidOperationException("Unknown Task state"));
            }

            //var taskResult = task.GetResult();
            //if (taskResults == null)
            //{
            //    var subTask = taskResult as Task;
            //    if (subTask != null)
            //        taskResult = subTask.GetResult();

            //    return callback(taskResult);
            //}


            //var taskResults = taskResult as Task[];
            //if (taskResults.Length == 0)
            //    return callback(TypeConstants.EmptyObjectArray);

            //var firstResponse = taskResults[0].GetResult();
            //var batchedResponses = firstResponse != null
            //    ? (object[])Array.CreateInstance(firstResponse.GetType(), taskResults.Length)
            //    : new object[taskResults.Length];
            //batchedResponses[0] = firstResponse;
            //for (var i = 1; i < taskResults.Length; i++)
            //{
            //    batchedResponses[i] = taskResults[i].GetResult();
            //}
            //return callback(batchedResponses);
        }

        //public object GetResult(Task task)
        //{
        //    try
        //    {
        //        task.Wait();

        //        Type taskType = task.GetType();
        //        if (!taskType.IsGenericType || taskType.FullName.Contains("VoidTaskResult"))
        //        {
        //            return null;
        //        }

        //        var fn = taskType.GetFastGetter("Result");
        //        return fn?.Invoke(task);
        //    }
        //    catch (TypeAccessException)
        //    {
        //        return null; //return null for void Task's
        //    }
        //    catch (Exception ex)
        //    {
        //        // throw ex.UnwrapIfSingleException();
        //    }
        //}

        internal static Type GetAsyncMethodTaskReturnType(MethodInfo method)
        {
            Type type = method.ReturnType;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>))
            {
                return type.GetGenericArguments()[0];
            }

            return null;
        }
    }
}