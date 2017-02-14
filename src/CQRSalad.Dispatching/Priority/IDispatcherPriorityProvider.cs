using System.Reflection;

namespace CQRSalad.Dispatching.Priority
{
    public interface IDispatcherPriorityProvider
    {
        Priority GetHandlerPriority(TypeInfo handlerType);
        Priority GetActionPriority(MethodInfo actionInfo);
    }
}