using System;
using System.Collections.Generic;
using System.Linq;
using CQRSalad.Dispatching.Payload;

namespace CQRSalad.Dispatching
{
    internal static class ScanningRuleExtensions
    {
        internal static List<Type> Scan(this ScanningRule rule)
        {
            List<Type> handlersTypes = rule.Assembly
                                           .GetExportedTypes()
                                           .Where(_type => _type.IsClass && rule.IsTypeInNamespaces(_type))
                                           .ToList();
            return handlersTypes;
        }

        private static bool IsTypeInNamespaces(this ScanningRule rule, Type type)
        {
            return rule.Namespaces.Count == 0 || rule.Namespaces.Any(x => type.FullName.StartsWith(x));
        }
    }
}