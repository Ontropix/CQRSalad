using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CQRSalad.EventSourcing.CodeGeneration;
using Microsoft.CSharp;

namespace CQRSalad.EventSourcing
{
    public static class ApplicationServiceGenerator
    {
        public static Assembly Generate(Assembly assemblyWithAggregates)
        {
            Argument.IsNotNull(assemblyWithAggregates, nameof(assemblyWithAggregates));

            List<Type> aggregateTypes = GetAggregateTypes(assemblyWithAggregates);
            string[] classSources = aggregateTypes.Select(x => new ApplicationServiceTemplate().Compile(x)).ToArray();
            
            string[] referencedAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                                                     .Where(a => !a.FullName.StartsWith("mscorlib", StringComparison.InvariantCultureIgnoreCase))
                                                     .Where(a => !a.IsDynamic)
                                                     .Select(a => a.Location)
                                                     .ToArray();

            var compilerParams = new CompilerParameters(referencedAssemblies) { GenerateInMemory = true };

            var providerOptions = new Dictionary<string, string> { { "CompilerVersion", "v4.0" } };
            var provider = new CSharpCodeProvider(providerOptions);
            CompilerResults results = provider.CompileAssemblyFromSource(compilerParams, classSources);
            return results.CompiledAssembly;
        }

        private static List<Type> GetAggregateTypes(Assembly assemblyWithAggregates)
        {
            return assemblyWithAggregates.GetExportedTypes().Where(x => x.IsClass && !x.IsAbstract && !x.IsGenericType && x.IsPublic
                                                                        && typeof (AggregateRoot).IsAssignableFrom(x)).ToList();
        }
    }
}
