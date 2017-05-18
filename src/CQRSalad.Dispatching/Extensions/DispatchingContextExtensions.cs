using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace CQRSalad.Dispatching
{
    // todo validate task result for another Task and more
    // todo throw good exceptions
    internal static class DispatchingContextExtensions
    {
        private static readonly ConcurrentDictionary<Type, Func<object, object>> _gettersCache =
            new ConcurrentDictionary<Type, Func<object, object>>();

        internal static Task Execute(this MessageInvoker invoker, DispatchingContext context)
        {
            var invocationResult = invoker(context.HandlerInstance, context.MessageInstance);

            var awaitableResult = invocationResult as Task;
            if (awaitableResult == null)
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
                    //TODO throw appropriate exception!
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

                context.Result = task.GetResult();
            });
        }

        private static object GetResult(this Task task)
        {
            var resultResolver = TaskResultResolver(task.GetType());
            var taskResult = resultResolver?.Invoke(task);
            return taskResult;
        }

        private static Func<object, object> TaskResultResolver(this Type taskType)
        {
            return _gettersCache.GetOrAdd(taskType, type =>
            {
                PropertyInfo property = taskType.GetProperty("Result");
                return property?.ValueGetterExpression(type).Compile();
            });
        }

        private static Expression<Func<object, object>> ValueGetterExpression(this PropertyInfo propertyInfo, Type type)
        {
            var instance = Expression.Parameter(typeof(object), "instance");
            var convertInstance = Expression.TypeAs(instance, type);
            var property = Expression.Property(convertInstance, propertyInfo);
            var convertProperty = Expression.TypeAs(property, typeof(object));
            return Expression.Lambda<Func<object, object>>(convertProperty, instance);
        }
    }
}