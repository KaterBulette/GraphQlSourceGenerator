using GraphQlSourceGenerator.Models.Interfaces;
using GraphQlSourceGenerator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphQlSourceGenerator.Models
{
    internal class GraphQlRequestInfo
    {
        public string Name { get; }

        public string AliasName { get; }

        public IList<(string ArgGqlTypeName, string ArgTypeName, string ArgName)> Args { get; }

        public IGraphQlObjectInfo ResponseType { get; }

        public string Document { get; }

        public GraphQlRequestInfo(GraphQlField gqlField, string documentTemplate)
        {
            _ = gqlField ?? throw new ArgumentNullException(nameof(gqlField));

            Name = gqlField.Name.ToCamelCase();
            AliasName = gqlField.Name;
            ResponseType = gqlField.GetResponseType();
            Args = gqlField.Args?
                           .Select(a => a.GetArgInfo())
                           .ToList();
            Document = Args.GetRequestDocument(AliasName, ResponseType, documentTemplate);
        }
    }
}
