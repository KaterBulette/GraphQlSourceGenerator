using GraphQlSourceGenerator.Enums;
using GraphQlSourceGenerator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphQlSourceGenerator.Models
{
    internal class GraphQlInterfaceInfo
    {
        public string Name { get; }

        public IList<string> ParentNames { get; }

        public IList<(GraphQlTypeKind FieldKind, string FieldTypeName, string FieldGqlName, string FieldName)> Fields { get; }

        public IList<string> PossibleTypes { get; }

        public GraphQlInterfaceInfo(GraphQlType gqlType)
        {
            _ = gqlType ?? throw new ArgumentNullException(nameof(gqlType));
            if (gqlType.Kind is not GraphQlTypeKind.Interface)
            {
                throw new NotSupportedException($"GraphQlInterfaceInfo not supported {gqlType.Kind} graphQl types");
            }

            Name = gqlType.Name.ToCamelCase();
            ParentNames = gqlType.Interfaces?
                                 .Select(i => i.Name.ToCamelCase())
                                 .ToList();
            Fields = gqlType.Fields?
                            .Select(f => f.GetFieldInfo())
                            .ToList();

            PossibleTypes = gqlType.PossibleTypes?
                                   .Select(pt => pt.UnwrapIfNonNull().Name)
                                   .ToList() ?? new List<string>();
        }
    }
}
