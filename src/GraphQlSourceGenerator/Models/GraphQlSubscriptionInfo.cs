using GraphQlSourceGenerator.Models.Interfaces;
using GraphQlSourceGenerator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphQlSourceGenerator.Models
{
    internal class GraphQlSubscriptionInfo
    {
        public string Name { get; }

        public string AliasName { get; }

        public IList<string> PossibleEventArgTypes { get; }

        public string Document { get; }

        public IGraphQlObjectInfo ResponseType { get; }

        public GraphQlSubscriptionInfo(GraphQlField gqlField, IList<GraphQlInterfaceInfo> interfaceInfos)
        {
            _ = gqlField ?? throw new ArgumentNullException(nameof(gqlField));
            _ = interfaceInfos ?? throw new ArgumentNullException(nameof(interfaceInfos));

            Name = gqlField.Name.ToCamelCase();
            AliasName = gqlField.Name;
            PossibleEventArgTypes = interfaceInfos.FirstOrDefault(ii => ii.Name.Equals(gqlField.Type.UnwrapIfNonNull().Name))
                                                 ?.PossibleTypes
                                                  .Select(pt => pt)
                                                  .ToList() ?? new List<string>();

            Document = PossibleEventArgTypes.GetSubscriptionDocument(AliasName);
            ResponseType = gqlField.GetResponseType();
        }
    }
}
