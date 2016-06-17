using System.Collections.Generic;
using System.Reflection;

namespace CQRSalad.Dispatching.NEW.TypesScanning
{
    public class AssemblyTypesProvider : IDispatcherTypesProvider
    {
        private readonly IEnumerable<AssemblyScanningRule> _rules;

        public AssemblyTypesProvider(IEnumerable<AssemblyScanningRule> rules)
        {
            _rules = rules;
        }

        public IEnumerable<TypeInfo> GetTypes()
        {
            List<TypeInfo> types = new List<TypeInfo>();
            foreach (AssemblyScanningRule rule in _rules)
            {
                types.AddRange(rule.ScanTypes());
            }
            return types;
        }
    }
}