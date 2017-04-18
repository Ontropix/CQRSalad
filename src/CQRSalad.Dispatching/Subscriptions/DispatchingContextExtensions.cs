using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace CQRSalad.Dispatching
{
    internal static class DispatchingContextExtensions
    {
        private static readonly ConcurrentDictionary<Type, Func<object, object>> _gettersCache =
            new ConcurrentDictionary<Type, Func<object, object>>();

        private const string ResultPropName = "Result";

        private static Func<object, object> GetTaskResultFunc(this Type taskType)
        {
            return _gettersCache.GetOrAdd(taskType, type =>
            {
                PropertyInfo property = taskType.GetProperty(ResultPropName);
                return property == null ? null : GetValueGetter(property, type);
            });
        }

        private static Func<object, object> GetValueGetter(this PropertyInfo propertyInfo, Type type)
        {
            var instance = Expression.Parameter(typeof(object), "instance");
            var convertInstance = Expression.TypeAs(instance, type);
            var property = Expression.Property(convertInstance, propertyInfo);
            var convertProperty = Expression.TypeAs(property, typeof(object));
            return Expression.Lambda<Func<object, object>>(convertProperty, instance).Compile();
        }

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

                var resultAccessor = GetTaskResultFunc(awaitableResult.GetType());
                var taskResult = resultAccessor?.Invoke(awaitableResult);

                if (taskResult is Task || task is IEnumerable<Task>)
                {
                    throw new InvalidOperationException();
                }

                context.Result = taskResult;
            });
        }
    }
}