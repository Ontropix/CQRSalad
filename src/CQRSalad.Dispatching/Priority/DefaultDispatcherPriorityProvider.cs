using System.Reflection;

namespace CQRSalad.Dispatching.Priority
{
    public class DefaultDispatcherPriorityProvider : IDispatcherPriorityProvider
    {
        private const Priority DefaultPriorty = Priority.Normal;

        public Priority GetHandlerPriority(TypeInfo handlerType)
        {
            return GetFromAttribute(handlerType);
        }

        public Priority GetActionPriority(MethodInfo actionInfo)
        {
            return GetFromAttribute(actionInfo);
        }

        private Priority GetFromAttribute(MemberInfo member)
        {
            if (member.IsDefined(typeof(DispatchingPriorityAttribute)))
            {
                var priorityAttribute = member.GetCustomAttribute<DispatchingPriorityAttribute>(false);
                return priorityAttribute.Priority;
            }

            return DefaultPriorty;
        }
    }
}