using GraphQlSourceGenerator.Models;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace GraphQlSourceGenerator.NET5.Utils
{
    internal static class GeneratorSettingsHelper
    {
        public static bool TryParse<TOut>(this ImmutableArray<AdditionalText> additionalFiles,
                                          string fileName,
                                          GeneratorExecutionContext context,
                                          CancellationToken token,
                                          out TOut output)
            where TOut : class
        {
            output = null;
            var settingsAdditionalFile = additionalFiles.FirstOrDefault(af => af.Path.Contains(fileName));

            if (settingsAdditionalFile != null)
            {
                var text = settingsAdditionalFile.GetText(token);
                try
                {
                    var serializerSettings = new JsonSerializerSettings
                    {
                        Converters = { new StringEnumConverter() }
                    };
                    var settings = JsonConvert.DeserializeObject<TOut>(text?.ToString(), serializerSettings);
                    if (settings != null)
                    {
                        output = settings;
                        return true;
                    }
                }
                catch (Exception e)
                {
                    context.AddExceptionReport(e);
                }
            }
            else
            {
                context.AddIoReport(fileName);
            }

            return false;
        }
    }
}
