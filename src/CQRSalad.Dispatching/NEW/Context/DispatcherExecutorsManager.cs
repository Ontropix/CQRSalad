using System;
using System.Linq.Expressions;
using System.Reflection;
using CQRSalad.Dispatching.NEW.Subscriptions;

namespace CQRSalad.Dispatching.NEW.Context
{
    internal delegate object HandlerExecutor(object handler, object message);

    internal class DispatcherExecutorsManager
    {
        private readonly DispatcherExecutorsCache _executorsCache;

        public DispatcherExecutorsManager(DispatcherExecutorsCache executorsCache)
        {
            _executorsCache = executorsCache;
        }

        internal ContextExecutor GetExecutor(DispatcherSubscription subscription)
        {
            //todo cache

            HandlerExecutor func = CreateExecutorDelegate(
                    subscription.HandlerType,
                    subscription.Action,
                    subscription.MessageType);

            bool isTaskResult = subscription.Action.IsAsync();
            var executor = new ContextExecutor(func, isTaskResult); //todo
            return executor;
        }

        private static HandlerExecutor CreateExecutorDelegate(Type handlerType, MethodInfo action, Type messageType)
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