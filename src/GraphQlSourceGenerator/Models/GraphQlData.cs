using Newtonsoft.Json;

namespace GraphQlSourceGenerator.Models
{
    internal class GraphQlData
    {
        [JsonProperty("__schema")]
        public GraphQlSchema Schema { get; set; }
    }
}
