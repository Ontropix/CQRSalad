using System;
using System.Collections.Generic;
using System.Reflection;

namespace CQRSalad.Dispatching
{
    public class DispatcherConfig
    {
        public IServiceProvider ServiceProvider { get; set; }

        public List<Assembly> AssembliesWithHandlers { get; set; }

        public List<Type> Interceptors { get; set; } = new List<Type>();

        public bool ThrowIfMultipleSendingHandlersFound { get; set; } = true;
    }
}