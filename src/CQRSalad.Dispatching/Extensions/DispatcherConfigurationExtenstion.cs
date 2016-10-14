using System;
using CQRSalad.Dispatching.Core;
using CQRSalad.Dispatching.Subscriptions;

namespace CQRSalad.Dispatching
{
    public static class DispatcherConfigurationExtensions
    {
        public static DispatcherConfiguration SetServiceProvider(
            this DispatcherConfiguration configuration, 
            IServiceProvider serviceProvider)
        {
            configuration.ServiceProvider = serviceProvider;
            return configuration;
        }

        public static DispatcherConfiguration SetSubscriptionStore(
            this DispatcherConfiguration configuration,
            DispatcherSubscriptionsStore store)
        {
            configuration.SubscriptionsStore = store;
            return configuration;
        }

        public static DispatcherConfiguration AddInterceptor(
            this DispatcherConfiguration configuration, 
            Type interceptorType)
        {
            configuration.Interceptors.Add(interceptorType);
            return configuration;
        }

        public static DispatcherConfiguration AddInterceptor<TInterceptor>(this DispatcherConfiguration configuration)
        {
            configuration.Interceptors.Add(typeof(TInterceptor));
            return configuration;
        }
    }
}
