using System;
using System.Collections.Generic;

namespace CQRSalad.Dispatching
{
    public interface IDispatcherTypesProvider
    {
        IEnumerable<Type> GetTypes();
    }
}