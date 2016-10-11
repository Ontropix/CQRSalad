using System;
using System.Collections.Generic;
using CQRSalad.Dispatching.ServiceProvider;
using CQRSalad.Dispatching.Subscriptions;

namespace CQRSalad.Dispatching.Core
{
    public class DispatcherConfiguration
    {
        public IDispatcherServiceProvider ServiceProvider { get; set; }

        public DispatcherSubscriptionsStore SubscriptionsStore { get; set; }

        public List<Type> Interceptors { get; set; } //todo type validation

        public DispatcherConfiguration()
        {
            Interceptors = new List<Type>();
        }
    }
}