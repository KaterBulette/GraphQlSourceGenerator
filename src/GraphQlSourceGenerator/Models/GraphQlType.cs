using System.Collections.Generic;

namespace GraphQlSourceGenerator.Models
{
    internal class GraphQlType : GraphQlTypeBase
    {
        public string Description { get; set; }
        public IList<GraphQlField> Fields { get; set; }
        public IList<GraphQlArgument> InputFields { get; set; }
        public IList<GraphQlFieldType> Interfaces { get; set; }
        public IList<GraphQlEnumValue> EnumValues { get; set; }
        public IList<GraphQlFieldType> PossibleTypes { get; set; }
    }
}
