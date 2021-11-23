﻿using System.Collections.Generic;

namespace GraphQlSourceGenerator.Models
{
    internal class GraphQlSchema
    {
        public GraphQlRequestType QueryType { get; set; }
        public IList<GraphQlType> Types { get; set; }
        public GraphQlRequestType MutationType { get; set; }
        public GraphQlRequestType SubscriptionType { get; set; }
    }
}
