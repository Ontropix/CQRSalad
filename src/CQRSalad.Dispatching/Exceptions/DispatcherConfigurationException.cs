using System;

namespace CQRSalad.Dispatching
{
    public class DispatcherConfigurationException : ApplicationException
    {
        public DispatcherConfig ConfigDump { get; private set; }

        public DispatcherConfigurationException(string message, DispatcherConfig configDump) : base(message)
        {
            ConfigDump = configDump;
        }
    }
}