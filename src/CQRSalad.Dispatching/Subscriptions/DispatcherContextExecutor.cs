using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSalad.Dispatching
{
    internal static class DispatchingContextExtensions
    {
        internal static Task Execute(this MessageInvoker invoker, DispatchingContext context)
        {
            var invocationResult = invoker(context.HandlerInstance, context.MessageInstance);

            var awaitableResult = invocationResult as Task;
            if (awaitableResult == null) //if not task - just assign the result to context
            {
                context.Result = invocationResult;
                return Task.CompletedTask;
            }

            if (awaitableResult.Status == TaskStatus.Created)
            {
                awaitableResult.Start();
            }

            return awaitableResult.ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    //throw taskResult.Exception;
                }

                if (task.IsCanceled)
                {
                    throw new OperationCanceledException("The async Task operation was cancelled");
                }

                if (!task.IsCompleted)
                {
                    throw new InvalidOperationException("Unknown Task state");
                }

                var taskResult = task.GetType().GetProperty("Result").GetValue(task);

                if (taskResult is Task || task is IEnumerable<Task>)
                {
                    throw new InvalidOperationException();
                }

                context.Result = taskResult;
            });
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