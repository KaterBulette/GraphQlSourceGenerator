using GraphQlSourceGenerator.Enums;

namespace GraphQlSourceGenerator.Models
{
    internal abstract class GraphQlTypeBase
    {
        public GraphQlTypeKind Kind { get; set; }
        public string Name { get; set; }
    }
}
