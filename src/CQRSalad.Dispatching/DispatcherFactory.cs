using System;
using CQRSalad.Dispatching.Async;

namespace CQRSalad.Dispatching
{
    public static class DispatcherFactory
    {
        public static MessageDispatcher Create(Action<DispatcherConfiguration> configuration)
        {
            return new MessageDispatcher(Configure(configuration));
        }

        public static AsyncMessageDispatcher CreateAsync(Action<DispatcherConfiguration> configuration)
        {
            return new AsyncMessageDispatcher(Configure(configuration));
        }

        private static DispatcherConfiguration Configure(Action<DispatcherConfiguration> configurationAction)
        {
            var config = new DispatcherConfiguration();
            configurationAction(config);
            config.Validate();
            return config;
        }
    }
}