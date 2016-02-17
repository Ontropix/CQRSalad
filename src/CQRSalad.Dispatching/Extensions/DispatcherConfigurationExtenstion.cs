using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CQRSalad.Dispatching.Payload;

namespace CQRSalad.Dispatching
{
    public static class DispatcherConfigurationExtensions
    {
        public static DispatcherConfiguration EnableHandlingPriority(this DispatcherConfiguration configuration)
        {
            configuration.IsHandlingPriorityEnabled = true;
            return configuration;
        }

        public static DispatcherConfiguration SetServiceLocator(this DispatcherConfiguration configuration, IServiceProvider serviceProvider)
        {
            configuration.ServiceLocator = serviceProvider;
            return configuration;
        }
        
        public static DispatcherConfiguration AddInterceptor(this DispatcherConfiguration configuration, Type interceptorType)
        {
            configuration.Interceptors.Add(interceptorType);
            return configuration;
        }

        public static DispatcherConfiguration AddInterceptor<TInterceptor>(this DispatcherConfiguration configuration)
        {
            Type interceptorType = typeof (TInterceptor);
            configuration.Interceptors.Add(interceptorType);
            return configuration;
        }

        public static DispatcherConfiguration AddScanRule(this DispatcherConfiguration configuration, Assembly assembly, List<string> namespaces = null)
        {
            configuration.ScanningRules.Add(new ScanningRule(assembly, namespaces));
            return configuration;
        }

        public static void Validate(this DispatcherConfiguration configuration)
        {
            var errors = new List<string>();

            if (configuration.ServiceLocator == null)
            {
                errors.Add("ServiceLocator is null.");
            }

            if (configuration.ScanningRules == null || !configuration.ScanningRules.Any())
            {
                errors.Add("No ScanRules were specified.");
            }

            if (configuration.Interceptors != null && configuration.Interceptors.Count > 0)
            {
                foreach (Type interceptorType in configuration.Interceptors)
                {
                    if (!typeof(IContextInterceptor).IsAssignableFrom(interceptorType))
                    {
                        errors.Add(String.Format("Interceptor {0} must implement IMessageHandlerInterceptor", interceptorType.FullName));
                    }
                }
            }

            if (errors.Any())
            {
                string errorMessage = String.Format("Configuration is invalid: \n - {0}", String.Join("\n - ", errors));
                throw new DispatcherConfigurationException(errorMessage, configuration);
            }
        }

    }
}
