using Microsoft.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using MobileAPI.Attributes;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Reflection;

namespace MobileAPI.Attributes
{
    public class CustomKeyValueFilter : ISchemaFilter
    {
        public void Apply(
            OpenApiSchema schema,
            SchemaFilterContext context)
        {
            var caProvider = context.MemberInfo
                ?? context.ParameterInfo
                as ICustomAttributeProvider;

            var attributes = caProvider?
                .GetCustomAttributes(true)
                .OfType<CustomKeyValueAttribute>();

            if (attributes != null)
            {
                foreach (var attribute in attributes)
                {
                    schema.Extensions.Add(
                        attribute.Key,
                        new OpenApiString(attribute.Value)
                        );
                }
            }
        }
    }
}