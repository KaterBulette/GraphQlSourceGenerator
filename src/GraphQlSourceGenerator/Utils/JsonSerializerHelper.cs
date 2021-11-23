using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;

namespace GraphQlSourceGenerator.Utils
{
    internal static class JsonSerializerHelper
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
    }
}
