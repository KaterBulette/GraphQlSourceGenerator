using System.Text;

namespace GraphQlSourceGenerator.Utils
{
    internal static class StringFormatter
    {
        public static string TrimLineEndings(this string inputValue)
        {
            return inputValue.TrimEnd().TrimStart('\r').TrimStart('\n');
        }

        public static void AppendLineIf(this StringBuilder sb, bool condition, string value)
        {
            if (condition)
            {
                sb.AppendLine(value);
            }
        }
    }
}
