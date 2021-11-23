using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using GraphQlSourceGenerator.Enums;
using GraphQlSourceGenerator.Models;
using GraphQlSourceGenerator.Models.Interfaces;

namespace GraphQlSourceGenerator.Utils
{
    internal static class GraphQlDataParser
    {
        private static readonly Regex ListTypeMatcher = new(@"List<(\w*)>");

        public static GraphQlSchema DeserializeGraphQlSchema(this string content)
        {
            try
            {
                var serializerSettings = new JsonSerializerSettings
                {
                    Converters = { new StringEnumConverter() }
                };
                var schema = JsonConvert.DeserializeObject<GraphQlResult>(content, serializerSettings)?.Data?.Schema
                          ?? JsonConvert.DeserializeObject<GraphQlData>(content, serializerSettings)?.Schema;

                if (schema == null)
                {
                    throw new ArgumentException("not a GraphQL schema", nameof(content));
                }

                return schema;
            }
            catch (JsonReaderException exception)
            {
                throw new ArgumentException("not a GraphQL schema", nameof(content), exception);
            }
        }

        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            var firstChar = value[0].ToString().ToUpperInvariant();
            return value.Length > 1
                 ? value.Substring(1).Insert(0, firstChar)
                 : firstChar;
        }

        public static (string ArgGqlTypeName, string ArgTypeName, string ArgName) GetArgInfo(this GraphQlArgument gqlArg)
        {
            _ = gqlArg ?? throw new ArgumentNullException(nameof(gqlArg));

            var (argTypeTemplate, argType) = gqlArg.Type.UnwrapIfNonNull() is { } unwrappedType && unwrappedType.Kind == GraphQlTypeKind.List
                ? (TemplatesContainer.ListTemplate, unwrappedType.Unwrap())
                : (TemplatesContainer.EmptyTemplate, gqlArg.Type.UnwrapIfNonNull());

            var argGqlTypeName = gqlArg.Type.Kind is GraphQlTypeKind.NonNull
                               ? $"{argType.Name}!"
                               : argType.Name;

            return (argGqlTypeName, string.Format(argTypeTemplate, argType.Name.ToCamelCase().CheckToReservedType()), gqlArg.Name);
        }

        public static GraphQlResponseInfo GetResponseType(this GraphQlField gqlField)
        {
            _ = gqlField ?? throw new ArgumentNullException(nameof(gqlField));

            var unwrappedType = gqlField.Type.UnwrapIfNonNull();
            var (responseTypeTemplate, responseType) = unwrappedType.Kind == GraphQlTypeKind.List
                ? (TemplatesContainer.ListTemplate, unwrappedType.Unwrap())
                : (TemplatesContainer.EmptyTemplate, unwrappedType);

            return new GraphQlResponseInfo(string.Format(TemplatesContainer.ResponseNameTemplate, gqlField.Name.ToCamelCase()),
                                           gqlField.Name,
                                           gqlField.Name.ToCamelCase(),
                                           string.Format(responseTypeTemplate, responseType.Name.ToCamelCase().CheckToReservedType()));
        }


        public static (GraphQlTypeKind FieldKind, string FieldTypeName, string FieldGqlName, string FieldName) GetFieldInfo(this GraphQlField gqlField)
        {
            _ = gqlField ?? throw new ArgumentNullException(nameof(gqlField));

            var (fieldTypeTemplate, fieldType) = gqlField.Type.Kind is GraphQlTypeKind.List
                ? (TemplatesContainer.ListTemplate, gqlField.Type.OfType)
                : (TemplatesContainer.EmptyTemplate, gqlField.Type);

            var fieldTypeName = (fieldType.Kind == GraphQlTypeKind.NonNull
                ? string.Format(fieldTypeTemplate, fieldType.UnwrapIfNonNull().Name.ToCamelCase())
                : string.Format(fieldTypeTemplate, fieldType.Name.ToCamelCase())).CheckToReservedType();

            var kind = fieldType.UnwrapIfNonNull().Kind;
            var fieldGqlName = gqlField.Name;
            var fieldName = gqlField.Name.ToCamelCase();

            return (kind, fieldTypeName, fieldGqlName, fieldName);
        }

        public static (GraphQlTypeKind FieldKind, string FieldTypeName, string FieldGqlName, string FieldName) GetFieldInfo(this GraphQlArgument gqlArgument)
        {
            _ = gqlArgument ?? throw new ArgumentNullException(nameof(gqlArgument));

            var (fieldTypeTemplate, fieldType) = gqlArgument.Type.Kind is GraphQlTypeKind.List
                            ? (TemplatesContainer.ListTemplate, gqlArgument.Type.OfType)
                            : (TemplatesContainer.EmptyTemplate, gqlArgument.Type);

            var fieldTypeName = (fieldType.Kind == GraphQlTypeKind.NonNull
                ? string.Format(fieldTypeTemplate, fieldType.UnwrapIfNonNull().Name.ToCamelCase())
                : string.Format(fieldTypeTemplate, fieldType.Name.ToCamelCase())).CheckToReservedType();

            var kind = fieldType.UnwrapIfNonNull().Kind;
            var fieldGqlName = gqlArgument.Name;
            var fieldName = gqlArgument.Name.ToCamelCase();

            return (kind, fieldTypeName, fieldGqlName, fieldName);
        }

        public static string GetRequestDocument(this IList<(string ArgGqlTypeName, string ArgTypeName, string ArgName)> args,
                                                string aliasName,
                                                IGraphQlObjectInfo responseType,
                                                string template)
        {
            _ = args ?? throw new ArgumentNullException(nameof(args));

            var aliasArgs = string.Join(", ", args.Select(a => $"${a.ArgName}: {a.ArgGqlTypeName}").ToList()).TrimEnd().TrimEnd(',');
            var requestArgs = string.Join(", ", args.Select(a => $"{a.ArgName}: ${a.ArgName}").ToList()).TrimEnd().TrimEnd(',');

            var unwrappedType = responseType.Fields.First().FieldTypeName.UnwrapIfList();
            var responseRef = unwrappedType.IsReservedType()
                            ? string.Empty
                            : string.Format(TemplatesContainer.GqlRequestReferenceTemplate, unwrappedType);

            return string.Format(template, aliasName, aliasArgs, requestArgs, responseRef);
        }

        public static string GetSubscriptionDocument(this IList<string> possibleEventArgTypes,
                                                     string aliasName)
        {
            _ = possibleEventArgTypes ?? throw new ArgumentNullException(nameof(possibleEventArgTypes));

            var eventArgs = possibleEventArgTypes.Select(a => string.Format(TemplatesContainer.GqlSubscriptionArgTemplate,
                                                              a,
                                                              string.Format(TemplatesContainer.GqlRequestReferenceTemplate, a)))
                                                 .ToList();
            var sb = new StringBuilder();
            eventArgs.ForEach(ea => sb.AppendLine("\t\t" + ea.Trim()));

            return string.Format(TemplatesContainer.GqlSubscriptionTemplate, aliasName, sb.ToString().Trim());
        }

        public static string GetClassDocument(this GraphQlType gqlType,
                                              IList<(GraphQlTypeKind FieldKind, string FieldTypeName, string FieldGqlName, string FieldName)> fields)
        {
            _ = gqlType ?? throw new ArgumentNullException(nameof(gqlType));
            if (fields?.Any() != true)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            foreach (var (fieldKind, fieldTypeName, fieldGqlName, _) in fields)
            {
                var unwrappedFieldTypeName = fieldTypeName.UnwrapIfList();
                var fragmentField = fieldKind is GraphQlTypeKind.Enum || unwrappedFieldTypeName.IsReservedType()
                                  ? fieldGqlName
                                  : $@"{fieldGqlName} {{{string.Format(TemplatesContainer.GqlFragmentReferenceTemplate, unwrappedFieldTypeName)}}}";
                sb.AppendLine("\t" + fragmentField.Trim());
            }

            return string.Format(TemplatesContainer.GqlFragmentTemplate, gqlType.Name.ToCamelCase(), sb.ToString().TrimLineEndings());
        }

        public static GraphQlFieldType UnwrapIfNonNull(this GraphQlFieldType graphQlType)
        {
            return graphQlType.Kind is GraphQlTypeKind.NonNull
                 ? graphQlType.OfType.UnwrapIfNonNull()
                 : graphQlType;
        }

        public static GraphQlFieldType UnwrapIfList(this GraphQlFieldType graphQlType)
        {
            return graphQlType.Kind is GraphQlTypeKind.List
                ? graphQlType.OfType.UnwrapIfList()
                : graphQlType;
        }

        public static GraphQlFieldType Unwrap(this GraphQlFieldType graphQlType)
        {
            return graphQlType.Kind switch
            {
                GraphQlTypeKind.List => graphQlType.OfType.UnwrapIfList().Unwrap(),
                GraphQlTypeKind.NonNull => graphQlType.OfType.UnwrapIfNonNull().Unwrap(),
                { } => graphQlType
            };
        }

        private static string UnwrapIfList(this string typeName)
        {
            return ListTypeMatcher.IsMatch(typeName)
                ? ListTypeMatcher.Match(typeName).Value.Replace("List<", "").Replace(">", "")
                : typeName;
        }

        private static string CheckToReservedType(this string typeName)
        {
            return typeName switch
            {
                "Boolean" => "bool",
                "String" => "string",
                "ID" => "string",
                "Int" => "int",
                { } => typeName,
                null => throw new ArgumentNullException(nameof(typeName))
            };
        }

        private static bool IsReservedType(this string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentNullException(nameof(typeName));
            }

            return typeName.ToLowerInvariant() switch
            {
                "boolean" => true,
                "bool" => true,
                "string" => true,
                "id" => true,
                "int" => true,
                { } => false
            };
        }
    }
}
