using GraphQlSourceGenerator.Enums;

namespace GraphQlSourceGenerator.Models
{
    public abstract class GraphQlTypeBase
    {
        public GraphQlTypeKind Kind { get; set; }
        public string Name { get; set; }
    }
}
