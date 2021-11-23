using GraphQlSourceGenerator.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.IO;

namespace GraphQlSourceGenerator.Utils
{
    public static class JsonSerializerHelper
    {
        public static TOut DeserializeFile<TOut>(this string settingsPath)
        where TOut : class
        {
            if (File.Exists(settingsPath))
            {
                var text = File.ReadAllText(settingsPath);

                var serializerSettings = new JsonSerializerSettings
                {
                    Converters = { new StringEnumConverter() }
                };
                return JsonConvert.DeserializeObject<TOut>(text, serializerSettings);
            }
            else
            {
                throw new FileNotFoundException(settingsPath);
            }
        }

        public static GraphQlSchema DeserializeGraphQlSchema(this string content)
        {
            try
            {
                var serializerSettings = new JsonSerializerSettings
                {
                    Converters = { new StringEnumConverter() }
                };
                var schema = JsonConvert.DeserializeObject<GraphQlResult>(content, serializerSettings)?.Data?.Schema
                             ?? JsonConvert.DeserializeObject<GraphQlData>(content, serializerSettings)?.Schema;

                if (schema == null)
                {
                    throw new ArgumentException("not a GraphQL schema", nameof(content));
                }

                return schema;
            }
            catch (JsonReaderException exception)
            {
                throw new ArgumentException("not a GraphQL schema", nameof(content), exception);
            }
        }
    }
}
