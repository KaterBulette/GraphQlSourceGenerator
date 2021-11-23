using GraphQlSourceGenerator.NET5.Utils;
using GraphQlSourceGenerator.Services;
using GraphQlSourceGenerator.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GraphQlSourceGenerator.Models;

namespace GraphQlSourceGenerator.NET5
{
    [Generator]
    public class GraphQlSourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
#if false
            _ = System.Diagnostics.Debugger.Launch();
#endif

            try
            {
                if (context.IsValidReferences()
                 && context.AdditionalFiles.TryParse<GraphQlGeneratorSettings>("GraphQlGeneratorSettings.json", context, context.CancellationToken, out var graphQlGeneratorSettings)
                 && context.AdditionalFiles.TryParse<GraphQlSchema>("schema.json", context, context.CancellationToken, out var schema))
                {
                    context.CancellationToken.ThrowIfCancellationRequested();

                    var generator = new Generator(schema, graphQlGeneratorSettings, nameof(GraphQlSourceGenerator));

                    context.CancellationToken.ThrowIfCancellationRequested();

                    IList<(string FileName, string FileContent)> filesInfo = generator.Generate();

                    context.CancellationToken.ThrowIfCancellationRequested();

                    foreach (var (fileName, fileContent) in filesInfo)
                    {
                        context.AddSource(fileName, SourceText.From(fileContent, Encoding.UTF8));
                    }
                }
            }
            catch (OperationCanceledException _)
            {
                //ignored
            }
            catch (Exception e)
            {
                context.AddExceptionReport(e);
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {

        }
    }
}
