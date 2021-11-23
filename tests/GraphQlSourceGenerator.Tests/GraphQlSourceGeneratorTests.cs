using Xunit;

namespace GraphQlSourceGenerator.Tests
{
    public class GraphQlSourceGeneratorTests
    {
        [Fact]
        public void GraphQlSourceGeneratorReturnFalseIfSchemaFileNotExist()
        {
            var target = CreateTarget();
            target.SchemaPath = "../schema.json";

            var result = target.Execute();

            Assert.False(result);
        }

        private GraphQlSourceGenerator CreateTarget()
        {
            return new();
        }
    }
}