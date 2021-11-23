using Newtonsoft.Json;

namespace GraphQlSourceGenerator.Models
{
    internal class GraphQlSchemaLoaderSettings
    {
        [JsonProperty("schemaUrl")]
        public string SchemaUrl { get; }

        [JsonProperty("authorizationJwtToken")]
        public string AuthorizationJwtToken { get; }

        public GraphQlSchemaLoaderSettings([JsonProperty("schemaUrl")]
                                           string schemaUrl,
                                           [JsonProperty("authorizationJwtToken")]
                                           string authorizationJwtToken)
        {
            SchemaUrl = schemaUrl;
            AuthorizationJwtToken = authorizationJwtToken;
        }
    }
}
