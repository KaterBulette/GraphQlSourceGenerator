namespace GraphQlSourceGenerator.Models
{
    internal class GraphQlArgument : GraphQlValueBase
    {
        public GraphQlFieldType Type { get; set; }
        public object DefaultValue { get; set; }
    }
}
