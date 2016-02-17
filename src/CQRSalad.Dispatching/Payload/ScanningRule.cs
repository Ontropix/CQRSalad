using System.Collections.Generic;
using System.Reflection;

namespace CQRSalad.Dispatching.Payload
{
    public class ScanningRule
    {
        public Assembly Assembly { get; private set; }
        public List<string> Namespaces { get; private set; }

        public ScanningRule(Assembly assembly, List<string> namespaces = null)
        {
            Assembly = assembly;
            Namespaces = namespaces ?? new List<string>();
        }
    }
}