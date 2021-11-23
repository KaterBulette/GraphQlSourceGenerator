using System;
using System.Collections.Generic;
using System.Linq;
using GraphQlSourceGenerator.Enums;
using GraphQlSourceGenerator.Utils;

namespace GraphQlSourceGenerator.Models
{
    internal class GraphQlEnumInfo
    {
        public string Name { get; }

        public IList<string> Values { get; }

        public GraphQlEnumInfo(GraphQlType gqlType)
        {
            _ = gqlType ?? throw new ArgumentNullException(nameof(gqlType));
            if (gqlType.Kind is not GraphQlTypeKind.Enum)
            {
                throw new NotSupportedException($"GraphQlEnumInfo not supported {gqlType.Kind} graphQl types");
            }

            Name = gqlType.Name.ToCamelCase();
            Values = gqlType.EnumValues
                            .Select(ev => ev.Name.ToCamelCase())
                            .ToList();
        }
    }
}
