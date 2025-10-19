using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MobileAPI.Swagger
{
    internal class CustomDocumentFilter : IDocumentFilter
    {
        public void Apply(
            OpenApiDocument swaggerDoc,
            DocumentFilterContext context)
        {
            swaggerDoc.Info.Title = "College Web API";
        }
    }
}