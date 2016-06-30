using System.Reflection;

namespace CQRSalad.Dispatching.NEW.Priority
{
    public enum DispatchingPriority
    {
        Unspecified = -1,
        Low = 1,
        Normal = 5,
        High = 7,
        Top = 10,
    }

    public interface IDispatcherPriorityProvider
    {
        DispatchingPriority GetHandlerPriority(TypeInfo handlerType);
        DispatchingPriority GetActionPriority(MethodInfo actionInfo);
    }
    
    public class DefaultDispatcherPriorityProvider : IDispatcherPriorityProvider
    {
        private const DispatchingPriority DefaultPriorty = DispatchingPriority.Unspecified;

        public DispatchingPriority GetHandlerPriority(TypeInfo handlerType)
        {
            if (handlerType.IsDefined(typeof(DispatchingPriorityAttribute)))
            {
                DispatchingPriorityAttribute priorityAttribute =
                    handlerType.GetCustomAttribute<DispatchingPriorityAttribute>(true);
                return priorityAttribute.Priority;
            }

            return DefaultPriorty;
        }

        public DispatchingPriority GetActionPriority(MethodInfo actionInfo)
        {
            if (actionInfo.IsDefined(typeof (DispatchingPriorityAttribute)))
            {
                DispatchingPriorityAttribute priorityAttribute =
                    actionInfo.GetCustomAttribute<DispatchingPriorityAttribute>(false);
                return priorityAttribute.Priority;
            }
            
            return DefaultPriorty;
        }
    }
}