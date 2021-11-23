using GraphQlSourceGenerator.Enums;
using GraphQlSourceGenerator.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace GraphQlSourceGenerator.Models
{
    internal class GraphQlResponseInfo : IGraphQlObjectInfo
    {
        public string Document => string.Empty;

        public string Name { get; }

        public IList<string> ParentNames => Enumerable.Empty<string>().ToList();

        public IList<(GraphQlTypeKind FieldKind, string FieldTypeName, string FieldGqlName, string FieldName)> Fields { get; }

        public GraphQlResponseInfo(string name, string fieldGqlName, string fieldName, string contentType)
        {
            Name = name;
            Fields = new List<(GraphQlTypeKind FieldKind, string FieldTypeName, string FieldGqlName, string FieldName)>()
            {
                (GraphQlTypeKind.Object, contentType, fieldGqlName, fieldName)
            };
        }
    }
}
