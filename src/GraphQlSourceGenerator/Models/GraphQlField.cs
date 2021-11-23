using System.Collections.Generic;

namespace GraphQlSourceGenerator.Models
{
    public class GraphQlField : GraphQlEnumValue
    {
        public IList<GraphQlArgument> Args { get; set; }
        public GraphQlFieldType Type { get; set; }
    }
}
