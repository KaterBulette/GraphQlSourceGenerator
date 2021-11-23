namespace GraphQlSourceGenerator.Models
{
    public class GraphQlArgument : GraphQlValueBase
    {
        public GraphQlFieldType Type { get; set; }
        public object DefaultValue { get; set; }
    }
}
