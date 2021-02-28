using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PokePlannerApi.OpenAPI
{
    public class RequiredAndNullableSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Properties == null)
            {
                return;
            }

            var requiredButNotNullableProperties = schema
               .Properties
               .Where(x => !x.Value.Nullable && schema.Required.Contains(x.Key))
               .ToList();

            foreach (var property in requiredButNotNullableProperties)
            {
                property.Value.Nullable = true;
            }
        }
    }
}
