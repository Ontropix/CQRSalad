using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CQRSalad.EventSourcing
{
    internal static class DelegateHelper
    {
        internal static TDelegate CreateMessageInvoker<TDelegate>(Type targetType, MethodInfo method, Type messageType)
        {
            Type objectType = typeof(object);
            ParameterExpression aggregateParameter = Expression.Parameter(objectType, "target");
            ParameterExpression commandParameter = Expression.Parameter(objectType, "message");

            MethodCallExpression methodCall =
                Expression.Call(
                    Expression.Convert(aggregateParameter, targetType),
                    method,
                    Expression.Convert(commandParameter, messageType));

            if (method.ReturnType != typeof(void))
            {
                throw new NotSupportedException("Only void return method allowed."); // todo
            }

            var lambda = Expression.Lambda<TDelegate>(
                methodCall,
                aggregateParameter,
                commandParameter);

            return lambda.Compile();
        }
    }
}