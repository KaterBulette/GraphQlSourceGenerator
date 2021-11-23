using GraphQlSourceGenerator.Enums;
using GraphQlSourceGenerator.Models.Interfaces;
using GraphQlSourceGenerator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphQlSourceGenerator.Models
{
    internal class GraphQlInputObjectInfo : IGraphQlObjectInfo
    {
        public string Document { get; }

        public string Name { get; }

        public IList<string> ParentNames { get; }

        public IList<(GraphQlTypeKind FieldKind, string FieldTypeName, string FieldGqlName, string FieldName)> Fields { get; }

        public GraphQlInputObjectInfo(GraphQlType gqlType)
        {
            _ = gqlType ?? throw new ArgumentNullException(nameof(gqlType));
            if (gqlType.Kind is not GraphQlTypeKind.InputObject)
            {
                throw new NotSupportedException($"GraphQlInputObjectInfo not supported {gqlType.Kind} graphQl types");
            }

            Name = gqlType.Name.ToCamelCase();
            ParentNames = gqlType.Interfaces?
                                 .Select(i => i.Name.ToCamelCase())
                                 .ToList();
            Fields = gqlType.InputFields?
                            .Select(f => f.GetFieldInfo())
                            .ToList();

            Document = gqlType.GetClassDocument(Fields);
        }
    }
}
