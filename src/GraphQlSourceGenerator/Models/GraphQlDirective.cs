using System.Collections.Generic;
using GraphQlSourceGenerator.Enums;

namespace GraphQlSourceGenerator.Models
{
    internal class GraphQlDirective
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<GraphQlDirectiveLocation> Locations { get; set; }
        public IList<GraphQlArgument> Args { get; set; }
    }
}
