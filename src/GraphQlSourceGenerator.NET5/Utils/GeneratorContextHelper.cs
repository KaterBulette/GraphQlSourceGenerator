using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphQlSourceGenerator.NET5.Utils
{
    internal static class GeneratorContextHelper
    {
        private static readonly DiagnosticDescriptor InvalidReferenceErrorDescriptor = new(id: "TCGQLGEN001",
                                                                                           title: "Invalid reference",
                                                                                           messageFormat: "Couldn't found reference '{0}'.",
                                                                                           category: "GraphQlSourceGenerator",
                                                                                           DiagnosticSeverity.Error,
                                                                                           isEnabledByDefault: true);

        private static readonly DiagnosticDescriptor CatchExceptionErrorDescriptor = new(id: "TCGQLGEN002",
                                                                                         title: "Unhandled exception",
                                                                                         messageFormat: "Unhandled exception '{0}'.",
                                                                                         category: "GraphQlSourceGenerator",
                                                                                         DiagnosticSeverity.Error,
                                                                                         isEnabledByDefault: true);

        private static readonly DiagnosticDescriptor FileNotFoundErrorDescriptor = new(id: "TCGQLGEN003",
                                                                                       title: "File not found",
                                                                                       messageFormat: "File '{0}' not found.",
                                                                                       category: "GraphQlSourceGenerator",
                                                                                       DiagnosticSeverity.Error,
                                                                                       isEnabledByDefault: true);

        public static bool IsValidReferences(this GeneratorExecutionContext context)
        {
            var generatorReferencesList = new List<string>
            {
                "Newtonsoft.Json"
            };

            var result = true;
            foreach (var generatorReference in generatorReferencesList)
            {
                if (!context.Compilation.ReferencedAssemblyNames.Any(ai => ai.Name.Equals(generatorReference, StringComparison.OrdinalIgnoreCase)))
                {
                    result = false;
                    context.ReportDiagnostic(Diagnostic.Create(InvalidReferenceErrorDescriptor, Location.None, generatorReference));
                }
            }

            return result;
        }

        public static void AddExceptionReport(this GeneratorExecutionContext context, Exception exception)
        {
            context.ReportDiagnostic(Diagnostic.Create(CatchExceptionErrorDescriptor, Location.None, exception));
        }

        public static void AddIoReport(this GeneratorExecutionContext context, string path)
        {
            context.ReportDiagnostic(Diagnostic.Create(FileNotFoundErrorDescriptor, Location.None, path));
        }
    }
}
