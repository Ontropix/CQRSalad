using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CQRSalad.Dispatching.NEW.TypesScanning
{
    internal static class AssemblyScanningRuleExtensions
    {
        internal static IEnumerable<TypeInfo> ScanTypes(this AssemblyScanningRule rule)
        {
            IEnumerable<TypeInfo> handlersTypes = rule.Assembly.DefinedTypes.Where(type => IsInNamespaces(type, rule.Namespaces));
            return handlersTypes;
        }

        private static bool IsInNamespaces(this TypeInfo type, IList<string> namespaces)
        {
            return namespaces.Count == 0 || type.Namespace != null && namespaces.Any(x => type.Namespace.StartsWith(x));
        }
    }
}