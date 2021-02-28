using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PokePlannerApi.OpenAPI
{
    /// <summary>
    /// Custom filter for OpenAPI schema generation.
    /// TODO: see if this works with nullable reference types instead of
    /// [Required] attributes
    /// </summary>
    public class NullablePrimitiveTypesSchemaFilter : ISchemaFilter
    {
        private static readonly HashSet<string> PrimitiveTypes = new HashSet<string>
        {
            "boolean",
            "integer",
            "number",
        };

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Properties == null)
            {
                return;
            }

            // make primitive non-nullable properties required
            var primitiveTypeProperties = schema
                .Properties
                .Where(p => PrimitiveTypes.Contains(p.Value.Type) && !p.Value.Nullable)
                .ToList();

            foreach (var p in primitiveTypeProperties)
            {
                schema.Required.Add(p.Key);
            }

            // make array properties required and non-nullable
            var arrayProperties = schema
                .Properties
                .Where(p => p.Value.Type?.Equals("array", StringComparison.OrdinalIgnoreCase) ?? false)
                .ToList();

            foreach (var p in arrayProperties)
            {
                schema.Required.Add(p.Key);
                p.Value.Nullable = false;
            }

            // convert optional nullable properties to non-nullable
            var optionalNullableProperties = schema
                .Properties
                .Where(p => !schema.Required.Contains(p.Key) && p.Value.Nullable)
                .ToList();

            foreach (var p in optionalNullableProperties)
            {
                p.Value.Nullable = false;
            }
        }
    }
}
