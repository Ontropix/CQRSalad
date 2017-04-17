using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CSharp;

namespace CQRSalad.EventSourcing.CodeGeneration
{
    public static class ApplicationServiceGenerator
    {
        public static Assembly Generate(Assembly assemblyWithAggregates)
        {
            Argument.IsNotNull(assemblyWithAggregates, nameof(assemblyWithAggregates));

            List<Type> aggregateTypes = GetAggregateTypes(assemblyWithAggregates);
            string[] classSources = aggregateTypes.Select(Generate).ToArray();
            
            string[] referencedAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                                                     .Where(a => !a.FullName.StartsWith("mscorlib", StringComparison.InvariantCultureIgnoreCase))
                                                     .Where(a => !a.IsDynamic)
                                                     .Select(a => a.Location)
                                                     .ToArray();

            string assemblyFolder = Path.GetDirectoryName(assemblyWithAggregates.Location) ?? Path.GetTempPath();
            string assemblyName = $"{assemblyWithAggregates.GetName().Name}.AppServices.dll";

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
            return assemblyWithAggregates
                .GetExportedTypes()
                .Where(x =>
                    x.IsClass &&
                    !x.IsAbstract &&
                    !x.IsGenericType &&
                    x.IsPublic &&
                    typeof(IAggregateRoot).IsAssignableFrom(x))
                .ToList();
        }

        private static string Generate(Type aggregateType)
        {
            List<Type> messageTypes = aggregateType.GetMethodsWithSingleArgument()
                .Where(method => typeof(ICommand).IsAssignableFrom(method.GetParameters()[0].ParameterType))
                .Select(method => method.GetParameters()[0].ParameterType)
                .ToList();

            var template = new ApplicationServiceTemplate
            {
                Session = new Dictionary<string, object>
                {
                    {"AggregateType", aggregateType},
                    {"MessageTypes",  messageTypes}
                }
            };

            template.Initialize();

            return template.TransformText().Replace("\n", "").Replace("\r", "").Replace("\t", "");
        }
    }
}
