using System;
using System.Collections.Generic;

namespace CQRSalad.Dispatching.Subscriptions
{
    public interface IDispatcherSubscriptionsStore
    {
        IEnumerable<DispatcherSubscription> Get(Type messageType);
    }
}