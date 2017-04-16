using System;
using System.Linq;
using System.Reflection;

namespace CQRSalad.EventSourcing
{
    internal static class AggregatesCache
    {
        private static readonly MessageInvokersCache<Action<object, object>, WhenMethod> _whenCache =
            new MessageInvokersCache<Action<object, object>, WhenMethod>(
                invoker =>
                {
                    var ctor = invoker.Method.GetCustomAttribute<AggregateCtorAttribute>(false);
                    return new WhenMethod
                    {
                        AggregateType = invoker.Method.DeclaringType,
                        CommandType = invoker.Method.GetParameters().First().ParameterType,
                        Invoker = invoker,
                        IsCtor = ctor != null
                    };
                });

        private static readonly MessageInvokersCache<Action<object, object>, StateOnMethod> _stateCache =
            new MessageInvokersCache<Action<object, object>, StateOnMethod>(
                invoker => new StateOnMethod
                {
                    StateType = invoker.Method.DeclaringType,
                    EventType = invoker.Method.GetParameters().First().ParameterType,
                    Invoker = invoker
                });


        internal static WhenMethod GetWhenMethod(Type aggregateType, Type commandType)
        {
            return _whenCache.GetMessageInvoker(aggregateType, commandType, invoker =>
            {
                var ctor = invoker.Method.GetCustomAttribute<AggregateCtorAttribute>(false);
                return new WhenMethod
                {
                    AggregateType = aggregateType,
                    CommandType = commandType,
                    Invoker = invoker,
                    IsCtor = ctor != null
                };
            });
        }

        internal static StateOnMethod GetStateOnMethod(Type stateType, Type eventType)
        {
            return _stateCache.GetMessageInvoker(stateType, eventType,
                invoker => new StateOnMethod
                {
                    StateType = stateType,
                    EventType = eventType,
                    Invoker = invoker
                });
        }
    }
}