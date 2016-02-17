using System;
using System.Collections.Generic;
using CQRSalad.Dispatching.Payload;

namespace CQRSalad.Dispatching
{
    public class DispatcherConfiguration
    {
        public bool IsHandlingPriorityEnabled { get; set; }

        public IServiceProvider ServiceLocator { get; set; }

        public List<Type> Interceptors { get; set; }

        public List<ScanningRule> ScanningRules { get; set; }
        
        public DispatcherConfiguration()
        {
            Interceptors = new List<Type>();
            ScanningRules = new List<ScanningRule>();
        }
    }
}