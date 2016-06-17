using System.Collections.Generic;
using System.Reflection;

namespace CQRSalad.Dispatching.NEW.TypesScanning
{
    public class AssemblyScanningRule
    {
        public Assembly Assembly { get; private set; }
        public IList<string> Namespaces { get; private set; }

        public AssemblyScanningRule(Assembly assembly, IList<string> namespaces = null)
        {
            Assembly = assembly;
            Namespaces = namespaces ?? new List<string>();
        }
    }
}