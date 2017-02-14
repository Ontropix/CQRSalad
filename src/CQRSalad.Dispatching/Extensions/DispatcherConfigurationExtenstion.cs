using System;

namespace CQRSalad.Dispatching
{
    public static class DispatcherConfigurationExtensions
    {
        public static DispatcherConfig SetServiceProvider(
            this DispatcherConfig config, 
            IServiceProvider serviceProvider)
        {
            config.ServiceProvider = serviceProvider;
            return config;
        }

        public static DispatcherConfig AddInterceptor(
            this DispatcherConfig config, 
            Type interceptorType)
        {
            config.Interceptors.Add(interceptorType);
            return config;
        }

        public static DispatcherConfig AddInterceptor<TInterceptor>(this DispatcherConfig config)
        {
            config.Interceptors.Add(typeof(TInterceptor));
            return config;
        }
    }
}
