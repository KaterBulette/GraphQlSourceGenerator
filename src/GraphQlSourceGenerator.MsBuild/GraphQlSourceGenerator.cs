using GraphQlSourceGenerator.Models;
using GraphQlSourceGenerator.Services;
using GraphQlSourceGenerator.Utils;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.IO;

namespace GraphQlSourceGenerator
{
    public class GraphQlSourceGenerator : Task
    {
        [Required]
        public string SettingsPath { get; set; }

        [Required]
        public string SchemaPath { get; set; }

        [Required]
        public string OutputFolder { get; set; }

        public override bool Execute()
        {
#if false
            _ = System.Diagnostics.Debugger.Launch();
#endif

            try
            {
                if (!File.Exists(SchemaPath))
                {
                    throw new FileNotFoundException($"{SchemaPath} file not found");
                }

                Log.LogMessage($"[GraphQlSourceGenerator.Execute] received settings path: {SettingsPath}, output folder: {OutputFolder}");
                if (SettingsPath.DeserializeFile<GraphQlGeneratorSettings>() is { } graphQlGeneratorSettings)
                {
                    var schemaContent = File.ReadAllText(SchemaPath);
                    var schema = schemaContent.DeserializeGraphQlSchema();
                    Log.LogMessage($"[GraphQlSourceGenerator.Execute] loaded schema: {schema}");

                    var generator = new Generator(schema, graphQlGeneratorSettings, nameof(GraphQlSourceGenerator));

                    IList<(string FileName, string FileContent)> filesInfo = generator.Generate();
                    Log.LogMessage($"[GraphQlSourceGenerator.Execute] generator generated: {filesInfo.Count} files");

                    if (Directory.Exists(OutputFolder))
                    {
                        Directory.Delete(OutputFolder, true);
                    }

                    Directory.CreateDirectory(OutputFolder);

                    foreach (var file in filesInfo)
                    {
                        var path = Path.Combine(OutputFolder, file.FileName);
                        Log.LogMessage($"[GraphQlSourceGenerator.Execute] create file: {path}");
                        using var writeStream = File.CreateText(path);
                        writeStream.Write(file.FileContent);
                        writeStream.Close();
                    }

                    return true;
                }

                throw new FormatException($"{SettingsPath} file is empty or has unknown format");
            }
            catch (Exception e)
            {
                // used for Unit tests
                if (BuildEngine != null)
                {
                    Log.LogErrorFromException(e);
                }

                return false;
            }
        }
    }
}
