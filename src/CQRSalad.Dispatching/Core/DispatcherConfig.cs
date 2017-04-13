using System;
using System.Collections.Generic;

namespace CQRSalad.Dispatching
{
    public class DispatcherConfig
    {
        public IServiceProvider ServiceProvider { get; set; }

        public List<Type> TypesToRegister { get; set; } = new List<Type>();

        public List<Type> Interceptors { get; set; } = new List<Type>();

        public bool ThrowIfMultipleSendingHandlersFound { get; set; } = true;

        public Func<Type, bool> HandlersTypesResolver { get; set; }
    }
}