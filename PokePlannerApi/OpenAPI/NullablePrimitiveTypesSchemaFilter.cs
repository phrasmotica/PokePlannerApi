using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PokePlannerApi.OpenAPI
{
    public class NullablePrimitiveTypesSchemaFilter : ISchemaFilter
    {
        private static readonly HashSet<string> PrimitiveTypes = new HashSet<string>
        {
            "boolean",
            "integer",
            "number",
            "string",
        };

        /// <summary>
        /// Ensures schema properties with nullable primitive types are made to
        /// be non-nullable. Works under the assumption that nullable primitive
        /// model properties are not decorated with
        /// <see cref="RequiredAttribute"/>.
        /// </summary>
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Properties == null)
            {
                return;
            }

            var primitiveTypeProperties = schema
               .Properties
               .Where(x => PrimitiveTypes.Contains(x.Value.Type) && x.Value.Nullable)
               .ToList();

            foreach (var property in primitiveTypeProperties)
            {
                property.Value.Nullable = false;
            }
        }
    }
}
