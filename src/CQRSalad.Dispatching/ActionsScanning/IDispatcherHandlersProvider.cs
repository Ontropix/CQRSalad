using System.Collections.Generic;
using System.Reflection;

namespace CQRSalad.Dispatching.ActionsScanning
{
    public interface IDispatcherHandlerActionsProvider
    {
        IEnumerable<MethodInfo> GetHandlerActions(TypeInfo handlerType);
    }
}
