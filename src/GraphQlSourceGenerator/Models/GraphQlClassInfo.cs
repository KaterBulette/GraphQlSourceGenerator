using System;
using System.Collections.Generic;
using System.Linq;
using GraphQlSourceGenerator.Enums;
using GraphQlSourceGenerator.Models.Interfaces;
using GraphQlSourceGenerator.Utils;

namespace GraphQlSourceGenerator.Models
{
    internal class GraphQlClassInfo : IGraphQlObjectInfo
    {
        public string Document { get; }

        public string Name { get; }

        public IList<string> ParentNames { get; }

        public IList<(GraphQlTypeKind FieldKind, string FieldTypeName, string FieldGqlName, string FieldName)> Fields { get; }

        public GraphQlClassInfo(GraphQlType gqlType)
        {
            _ = gqlType ?? throw new ArgumentNullException(nameof(gqlType));
            if (gqlType.Kind is GraphQlTypeKind.Interface
                             or GraphQlTypeKind.Enum
                             or GraphQlTypeKind.InputObject)
            {
                throw new NotSupportedException($"GraphQlClassInfo not supported {gqlType.Kind} graphQl types");
            }

            Name = gqlType.Name.ToCamelCase();
            ParentNames = gqlType.Interfaces?
                                 .Select(i => i.Name.ToCamelCase())
                                 .ToList();
            Fields = gqlType.Fields?
                            .Select(f => f.GetFieldInfo())
                            .ToList();

            Document = gqlType.GetClassDocument(Fields);
        }
    }
}
