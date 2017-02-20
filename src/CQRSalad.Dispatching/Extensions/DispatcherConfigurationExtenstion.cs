using System;
using System.Collections.Generic;
using System.Reflection;

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

        public static DispatcherConfig RegisterHandlers(this DispatcherConfig config, IEnumerable<Type> types)
        {
            config.TypesToRegister.AddRange(types);
            return config;
        }

        public static DispatcherConfig RegisterHandlers(this DispatcherConfig config, Assembly assembly)
        {
            config.TypesToRegister.AddRange(assembly.GetExportedTypes());
            return config;
        }
    }
}
