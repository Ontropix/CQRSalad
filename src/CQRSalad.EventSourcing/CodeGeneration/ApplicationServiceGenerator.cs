using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CQRSalad.EventSourcing.CodeGeneration;
using Microsoft.CSharp;

namespace CQRSalad.EventSourcing
{
    public static class ApplicationServiceGenerator
    {
        public static Assembly Generate(Assembly aggregatesAssembly)
        {
            Argument.IsNotNull(aggregatesAssembly, nameof(aggregatesAssembly));

            List<Type> aggregateTypes = GetAggregateTypes(aggregatesAssembly);
            string[] classSources = aggregateTypes.Select(x => new ApplicationServiceTemplate().Compile(x)).ToArray();
            
            string[] referencedAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                                                     .Where(a => !a.FullName.StartsWith("mscorlib", StringComparison.InvariantCultureIgnoreCase))
                                                     .Where(a => !a.IsDynamic)
                                                     .Select(a => a.Location)
                                                     .ToArray();

            string assemblyFolder = Path.GetDirectoryName(aggregatesAssembly.Location) ?? Path.GetTempPath();
            string assemblyName = $"{aggregatesAssembly.GetName().Name}.Interface.dll";

            var compilerParams = new CompilerParameters(referencedAssemblies)
            {
                OutputAssembly = Path.Combine(assemblyFolder, assemblyName)
            };

            var providerOptions = new Dictionary<string, string> { { "CompilerVersion", "v4.0" } };
            var provider = new CSharpCodeProvider(providerOptions);
            CompilerResults results = provider.CompileAssemblyFromSource(compilerParams, classSources);
            return results.CompiledAssembly;
        }

        private static List<Type> GetAggregateTypes(Assembly assemblyWithAggregates)
        {
            return assemblyWithAggregates.GetExportedTypes().Where(x => x.IsClass && !x.IsAbstract && !x.IsGenericType && x.IsPublic
                                                                        && typeof (IAggregateRoot).IsAssignableFrom(x)).ToList();
        }
    }
}
