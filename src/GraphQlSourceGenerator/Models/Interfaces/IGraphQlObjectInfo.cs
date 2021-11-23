using GraphQlSourceGenerator.Enums;
using System.Collections.Generic;

namespace GraphQlSourceGenerator.Models.Interfaces
{
    internal interface IGraphQlObjectInfo
    {
        string Document { get; }

        string Name { get; }

        IList<string> ParentNames { get; }

        IList<(GraphQlTypeKind FieldKind, string FieldTypeName, string FieldGqlName, string FieldName)> Fields { get; }
    }
}
