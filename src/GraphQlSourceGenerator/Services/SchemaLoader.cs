using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GraphQlSourceGenerator.Services
{
    public class SchemaLoader
    {
        private readonly string _url;
        private readonly int _innerLevelOfType;
        private readonly HttpClient _httpClient = new();
        private readonly Dictionary<string, string> _headers;

        public SchemaLoader(string url, int innerLevelOfType, string jwtToken)
        {
            _url = url ?? throw new ArgumentNullException(nameof(url));
            _innerLevelOfType = innerLevelOfType;
            if (string.IsNullOrEmpty(jwtToken))
            {
                throw new ArgumentNullException(nameof(jwtToken));
            }

            const string authorizationSchema = "Bearer";
            const string authorizationHeader = "Authorization";
            _headers = new Dictionary<string, string> { { authorizationHeader, $"{authorizationSchema} {jwtToken}" } };
        }

        public async Task<string> LoadSchemaDataAsync()
        {
            var url = _url + "?&query=" + RetrieveSchemaQuery(_innerLevelOfType);
            using var request = new HttpRequestMessage(System.Net.Http.HttpMethod.Get, url);

            if (_headers != null)
            {
                foreach (var kvp in _headers)
                {
                    request.Headers.TryAddWithoutValidation(kvp.Key, kvp.Value);
                }
            }

            using var response = await _httpClient.SendAsync(request);

            var content = response.Content == null
                            ? "(no content)"
                            : await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"Status code: {(int) response.StatusCode} ({response.StatusCode}){Environment.NewLine}content:{Environment.NewLine}{content}");
            }

            return content;
        }

        private string RetrieveSchemaQuery(int innerLevelOfType)
        {
            string WrapTypeRefRequest()
            {
                const string ofTypeRequest =
                    @"ofType {{
  kind
  name
  {0}
}}";
                var result = string.Empty;

                for (var i = 0; i < innerLevelOfType - 1; i++)
                {
                    result = string.Format(ofTypeRequest, result);
                }

                return result;
            }

            var shortType = WrapTypeRefRequest();

            var request =
                @"query IntrospectionQuery {
  __schema {
    queryType {
      name
    }
    mutationType {
      name
    }
    subscriptionType {
      name
    }
    types {
      ...Type
    }
  }
}

fragment TypeRef on __Type {
  kind
  name
  " + shortType + @"
}

fragment Type on __Type {
  kind
  name
  description
  ofType {
    ...TypeRef
  }
  fields(includeDeprecated: true) {
    ...Field
  }
  inputFields {
    ...InputValue
  }
  interfaces {
    ...TypeRef
  }
  enumValues(includeDeprecated: true) {
    ...EnumValue
  }
  possibleTypes {
    ...TypeRef
  }
}

fragment Field on __Field {
  name
  description
  args {
    ...InputValue
  }
  type {
    ...TypeRef
  }
  isDeprecated
  deprecationReason
}

fragment InputValue on __InputValue {
  name
  description
  type {
    ...TypeRef
  }
  defaultValue
}

fragment EnumValue on __EnumValue {
  name
  description
  isDeprecated
  deprecationReason
}";

            return Regex.Replace(request, "[\r\n ]+", " ");
        }
    }
}
