using System;
using System.Collections.Generic;
using CQRSalad.Dispatching.Context;
using CQRSalad.Dispatching.Subscriptions;

namespace CQRSalad.Dispatching.Core
{
    public class DispatcherConfiguration
    {
        public IServiceProvider ServiceProvider { get; set; }

        public DispatcherSubscriptionsStore SubscriptionsStore { get; set; }

        internal DispatcherExecutorsManager ExecutorManager { get; set; } = new DispatcherExecutorsManager();

        public List<Type> Interceptors { get; set; } = new List<Type>();
    }
}