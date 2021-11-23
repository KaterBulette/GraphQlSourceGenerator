namespace GraphQlSourceGenerator.Models
{
    internal class GraphQlEnumValue : GraphQlValueBase
    {
        public bool IsDeprecated { get; set; }
        public string DeprecationReason { get; set; }
    }
}
