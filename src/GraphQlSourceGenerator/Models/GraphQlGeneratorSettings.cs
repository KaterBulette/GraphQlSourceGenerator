using Newtonsoft.Json;
using System.Collections.Generic;

namespace GraphQlSourceGenerator.Models
{
    internal class GraphQlGeneratorSettings
    {
        [JsonProperty("namespace")]
        public string Namespace { get; }

        [JsonProperty("usings")]
        public IList<string> Usings { get; }

        public GraphQlGeneratorSettings([JsonProperty("namespace")]
                                        string nameSpace,
                                        [JsonProperty("usings")]
                                        IList<string> usings)
        {
            Namespace = nameSpace;
            Usings = usings;
        }
    }
}
