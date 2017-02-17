using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CQRSalad.Dispatching
{
    public class AssemblyTypesProvider
    {
        private readonly IEnumerable<Assembly> _assemblies;

        public AssemblyTypesProvider(IEnumerable<Assembly> assemblies)
        {
            _assemblies = assemblies;
        }

        public IEnumerable<Type> GetTypes()
        {
            return _assemblies.SelectMany(x => x.ExportedTypes);
        }
    }
}