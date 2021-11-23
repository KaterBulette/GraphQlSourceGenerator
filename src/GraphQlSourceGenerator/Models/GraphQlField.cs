using System.Collections.Generic;

namespace GraphQlSourceGenerator.Models
{
    internal class GraphQlField : GraphQlEnumValue
    {
        public IList<GraphQlArgument> Args { get; set; }
        public GraphQlFieldType Type { get; set; }
    }
}
