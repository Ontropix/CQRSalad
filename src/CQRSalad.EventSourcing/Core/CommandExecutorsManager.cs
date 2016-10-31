using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CQRSalad.EventSourcing
{
    internal delegate void CommandExecutor(DomainContext context); //object, object

    internal class CommandExecutorsManager
    {
        internal CommandExecutor GetExecutor(Type aggregateType, Type commandType)
        {
            //todo cache

            MethodInfo action = aggregateType.FindMethodBySinglePameter(commandType);
            CommandExecutor executor = CreateExecutorDelegate(aggregateType, action, commandType);

            var executor = new DispatcherContextExecutor(func, isTaskResult); //todo

            return executor;
        }

        private static CommandExecutor CreateExecutorDelegate(Type aggregateType, MethodInfo action, Type commandType)
        {
            Type objectType = typeof(object);
            ParameterExpression aggregateParameter = Expression.Parameter(objectType, "aggregate");
            ParameterExpression commandParameter = Expression.Parameter(objectType, "command");

            MethodCallExpression methodCall =
                Expression.Call(
                    Expression.Convert(aggregateParameter, aggregateType),
                    action,
                    Expression.Convert(commandParameter, commandType));

            if (action.ReturnType != typeof(void))
            {
                throw new NotSupportedException("Only void return method allowed."); // todo
            }

            var lambda = Expression.Lambda<CommandExecutor>(
                methodCall,
                aggregateParameter,
                commandParameter);

            return lambda.Compile();
        }
    }
}