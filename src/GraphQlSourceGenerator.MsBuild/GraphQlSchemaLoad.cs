using GraphQlSourceGenerator.Models;
using GraphQlSourceGenerator.Services;
using GraphQlSourceGenerator.Utils;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.IO;
using System.Threading;

namespace GraphQlSourceGenerator
{
    public class GraphQlSchemaLoad : Task
    {

        [Required]
        public string SettingsPath { get; set; }

        [Required]
        public string OutputFilePath { get; set; }

        public override bool Execute()
        {
            Log.LogMessage("[SchemaDownload.Execute] run");

            try
            {
                Log.LogCommandLine($"[SchemaDownload.Execute] received settings path: {SettingsPath}, output file: {OutputFilePath}");
                if (SettingsPath.DeserializeFile<GraphQlSchemaLoaderSettings>() is { } graphQlGeneratorSettings)
                {
                    var schemaLoader = new SchemaLoader($@"{graphQlGeneratorSettings.SchemaUrl}", 6, graphQlGeneratorSettings.AuthorizationJwtToken);
                    var loadTask = schemaLoader.LoadSchemaDataAsync();
                    var cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromSeconds(10));
                    System.Threading.Tasks.Task.WaitAny(new System.Threading.Tasks.Task[] { loadTask }, cts.Token);

                    var schema = loadTask.Result;
                    Log.LogMessage($"[SchemaDownload.Execute] loaded schema: {schema}");

                    if (!string.IsNullOrEmpty(schema))
                    {
                        if (File.Exists(OutputFilePath))
                        {
                            File.Delete(OutputFilePath);
                        }

                        Log.LogMessage($"[SchemaDownload.Execute] create file: {OutputFilePath}");
                        using var writeStream = File.CreateText(OutputFilePath);
                        writeStream.Write(schema);
                        writeStream.Close();
                    }
                    return true;
                }

                throw new FormatException("GraphQlSchemaLoaderSettings file is empty or has unknown format");
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e);
                return false;
            }
        }
    }
}
