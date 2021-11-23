using System.Runtime.Serialization;

namespace GraphQlSourceGenerator.Enums
{
    internal enum GraphQlDirectiveLocation
    {
        /// <summary>
        /// Location adjacent to a query operation.
        /// </summary>
        [EnumMember(Value = "QUERY")] Query,
        /// <summary>
        /// Location adjacent to a mutation operation.
        /// </summary>
        [EnumMember(Value = "MUTATION")] Mutation,
        /// <summary>
        /// Location adjacent to a subscription operation.
        /// </summary>
        [EnumMember(Value = "SUBSCRIPTION")] Subscription,
        /// <summary>
        /// Location adjacent to a field.
        /// </summary>
        [EnumMember(Value = "FIELD")] Field,
        /// <summary>
        /// Location adjacent to a fragment definition.
        /// </summary>
        [EnumMember(Value = "FRAGMENT_DEFINITION")] FragmentDefinition,
        /// <summary>
        /// Location adjacent to a fragment spread.
        /// </summary>
        [EnumMember(Value = "FRAGMENT_SPREAD")] FragmentSpread,
        /// <summary>
        /// Location adjacent to an inline fragment.
        /// </summary>
        [EnumMember(Value = "INLINE_FRAGMENT")] InlineFragment,
        /// <summary>
        /// Location adjacent to a variable definition.
        /// </summary>
        [EnumMember(Value = "VARIABLE_DEFINITION")] VariableDefinition,
        /// <summary>
        /// Location adjacent to a schema definition.
        /// </summary>
        [EnumMember(Value = "SCHEMA")] Schema,
        /// <summary>
        /// Location adjacent to a scalar definition.
        /// </summary>
        [EnumMember(Value = "SCALAR")] Scalar,
        /// <summary>
        /// Location adjacent to an object type definition.
        /// </summary>
        [EnumMember(Value = "OBJECT")] Object,
        /// <summary>
        /// Location adjacent to a field definition.
        /// </summary>
        [EnumMember(Value = "FIELD_DEFINITION")] FieldDefinition,
        /// <summary>
        /// Location adjacent to an argument definition.
        /// </summary>
        [EnumMember(Value = "ARGUMENT_DEFINITION")] ArgumentDefinition,
        /// <summary>
        /// Location adjacent to an interface definition.
        /// </summary>
        [EnumMember(Value = "INTERFACE")] Interface,
        /// <summary>
        /// Location adjacent to a union definition.
        /// </summary>
        [EnumMember(Value = "UNION")] Union,
        /// <summary>
        /// Location adjacent to an enum definition.
        /// </summary>
        [EnumMember(Value = "ENUM")] Enum,
        /// <summary>
        /// Location adjacent to an enum value definition.
        /// </summary>
        [EnumMember(Value = "ENUM_VALUE")] EnumValue,
        /// <summary>
        /// Location adjacent to an input object type definition.
        /// </summary>
        [EnumMember(Value = "INPUT_OBJECT")] InputObject,
        /// <summary>
        /// Location adjacent to an input field definition.
        /// </summary>
        [EnumMember(Value = "INPUT_FIELD_DEFINITION")] InputFieldDefinition
    }
}
