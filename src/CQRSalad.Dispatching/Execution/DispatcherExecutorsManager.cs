using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CQRSalad.Dispatching
{
    internal delegate object HandlerExecutor(object handler, object message);

    internal class DispatcherExecutorsManager
    {
        private readonly DispatcherExecutorsCache _executorsCache;

        internal IDispatcherContextExecutor GetExecutor(DispatcherSubscription subscription)
        {
            //todo cache

            HandlerExecutor func = CreateExecutorDelegate(
                subscription.MessageType,
                subscription.HandlerType,
                subscription.Action);

            bool isTaskResult = subscription.Action.IsAsync();

            if (isTaskResult)
            {
                return new AsyncContextExecutor(func);
            }

            return new SyncContextExecutor(func);
        }

        private static HandlerExecutor CreateExecutorDelegate(Type messageType, Type handlerType, MethodInfo action)
        {
            Type objectType = typeof(object);
            ParameterExpression handlerParameter = Expression.Parameter(objectType, "handler");
            ParameterExpression messageParameter = Expression.Parameter(objectType, "message");

            MethodCallExpression methodCall =
                Expression.Call(
                    Expression.Convert(handlerParameter, handlerType),
                    action,
                    Expression.Convert(messageParameter, messageType));

            if (action.ReturnType == typeof(void))
            {
                var lambda = Expression.Lambda<Action<object, object>>(
                    methodCall,
                    handlerParameter,
                    messageParameter);

                Action<object, object> voidExecutor = lambda.Compile();
                return (handler, message) =>
                {
                    voidExecutor(handler, message);
                    return null;
                };
            }
            else
            {
                var lambda = Expression.Lambda<HandlerExecutor>(
                    Expression.Convert(methodCall, typeof(object)),
                    handlerParameter,
                    messageParameter);

                return lambda.Compile();
            }
        }
    }
}