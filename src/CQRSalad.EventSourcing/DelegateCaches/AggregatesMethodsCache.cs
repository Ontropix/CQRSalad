using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace CQRSalad.EventSourcing
{
    internal delegate void CommandHandler(object aggregate, object command);

    internal class CommandHandlerSubscription
    {
        public Type AggregateType { get; set; }
        public Type CommandType { get; set; }
        public CommandHandler Handler { get; set; }
        public bool IsCtor { get; set; }
    }

    internal static class AggregatesMethodsCache
    {
        // Command Type - Aggregate method delegate
        private static readonly ConcurrentDictionary<Type, CommandHandlerSubscription> _cache = new ConcurrentDictionary<Type, CommandHandlerSubscription>();

        internal static CommandHandlerSubscription GetCommandHandler(Type aggregateType, Type commandType)
        {
            if (_cache.ContainsKey(commandType))
            {
                return _cache[commandType];
            }

            MethodInfo action = aggregateType.FindMethodBySinglePameter(commandType);
            if (action == null)
            {
                return null;
            }

            var ctor = action.GetCustomAttribute<AggregateCtorAttribute>(false);
            var handler = DelegateHelper.CreateMessageInvoker<CommandHandler>(aggregateType, action, commandType);

            var subscription = new CommandHandlerSubscription
            {
                AggregateType = aggregateType,
                CommandType = commandType,
                Handler = handler,
                IsCtor = ctor != null
            };

            _cache.TryAdd(commandType, subscription);
            return subscription;
        }
    }

    internal class MethodsCache
    {

    }
}