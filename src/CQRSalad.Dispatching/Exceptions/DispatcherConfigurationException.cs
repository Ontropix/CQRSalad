using System;
using CQRSalad.Dispatching.Core;

namespace CQRSalad.Dispatching
{
    public class DispatcherConfigurationException : ApplicationException
    {
        public DispatcherConfiguration ConfigurationDump { get; private set; }

        public DispatcherConfigurationException(string message, DispatcherConfiguration configurationDump) : base(message)
        {
            ConfigurationDump = configurationDump;
        }
    }
}