using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CQRSalad.Dispatching.Payload;
using CQRSalad.Dispatching.Priority;

namespace CQRSalad.Dispatching
{
    internal class HandlersRegistry
    {
        private const ExecutionPriority DefaultExecutionPriorty = ExecutionPriority.Normal;
        
        private readonly bool _isHandlingPriorityEnabled;
        private readonly ConcurrentDictionary<Type, List<MessageSubscription>> _subscriptionsMap;

        internal HandlersRegistry(bool isHandlingPriorityEnabled)
        {
            _isHandlingPriorityEnabled = isHandlingPriorityEnabled;
            _subscriptionsMap = new ConcurrentDictionary<Type, List<MessageSubscription>>();
        }

        internal void RegisterHandlers(List<ScanningRule> scanningRules)
        {
            List<Type> handlersTypes = new List<Type>();
            foreach (ScanningRule scanningRule in scanningRules)
            {
                handlersTypes.AddRange(scanningRule.Scan()
                                                   .Where(_class => typeof(IDispatcherHandler).IsAssignableFrom(_class) &&
                                                                    !_class.IsAbstract &&
                                                                    !_class.IsGenericTypeDefinition &&
                                                                    !_class.ContainsGenericParameters));
            }

            List<MethodInfo> handlersMethods = handlersTypes.Distinct().SelectMany(GetHandlerMethods).ToList();

            handlersMethods.GroupBy(_method => _method.GetParameters()[0].ParameterType)
                           .ToList()
                           .ForEach(group => _subscriptionsMap[group.Key] = group.Select(CreateSubscription).ToList());
            
            if (_isHandlingPriorityEnabled)
            {
                SortSubscriptionsByPriority();
            }
        }
        
        internal List<MessageSubscription> GetSubscriptions(Type messageType)
        {
            if (!_subscriptionsMap.ContainsKey(messageType))
            {
                throw new HandlerNotFoundException(messageType);
            }
            return _subscriptionsMap[messageType];
        }

        private List<MethodInfo> GetHandlerMethods(Type handlerType)
        {
            return handlerType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(MethodsFilter).ToList();
        }

        protected virtual bool MethodsFilter(MethodInfo method)
        {
            bool isDefinitionMatch = method.IsPublic &&
                                     method.GetParameters().Length == 1 &&
                                     !method.IsAbstract &&
                                     !method.ContainsGenericParameters &&
                                     !method.IsConstructor &&
                                     !method.IsGenericMethod &&
                                     !method.IsStatic;

            return isDefinitionMatch;
        }

        private MessageSubscription CreateSubscription(MethodInfo method)
        {
            return new MessageSubscription
            {
                HandlerType = method.DeclaringType,
                HandlerMethod = method,
                MessageType = method.GetParameters()[0].ParameterType,
                ReturnType = method.ReturnType,
                Priority = ExtractHandlerPriorty(method)
            };
        }
        
        private void SortSubscriptionsByPriority()
        {
            foreach (Type messageType in _subscriptionsMap.Keys)
            {
                _subscriptionsMap[messageType] = _subscriptionsMap[messageType].OrderByDescending(x => x.Priority).ToList();
            }
        }

        private ExecutionPriority ExtractHandlerPriorty(MethodInfo method)
        {
            if (!_isHandlingPriorityEnabled)
            {
                return DefaultExecutionPriorty;
            }

            //if we did not find attribute in method -- we try to find it in class
            ExecutionPriorityAttribute priorityAttribute = method.FirstOrDefaultAttribute<ExecutionPriorityAttribute>(lookInherited: true) ??
                                                           method.ReflectedType.FirstOrDefaultAttribute<ExecutionPriorityAttribute>(lookInherited: true);
            
            ExecutionPriority priority = priorityAttribute == null ? DefaultExecutionPriorty : priorityAttribute.ExecutionPriority;
            return priority;
        }
    }
}
