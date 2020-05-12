using Microsoft.OpenApi.Models;
using Swagger.Extensions.DependencyInjection.Configurations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Microsoft.Extensions.DependencyInjection {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddDefaultSwaggerService(this IServiceCollection serviceDescriptors, SwaggerDocSettings swaggerDocSettings) =>
            serviceDescriptors.AddSwaggerGen(c => {
                c.SwaggerDoc(swaggerDocSettings.DocName, new OpenApiInfo { Title = swaggerDocSettings.Info.Title, Version = swaggerDocSettings.Info.Version });
                c.CustomOperationIds(apiDesc => apiDesc.TryGetMethodInfo(out var methodInfo) ? methodInfo.Name.RemoveFromEnd("Async") : null);
                c.DocumentFilter<AdditionalParametersDocumentFilter>();
            });

        internal static string RemoveFromEnd(this string target, string suffix) =>
            target.EndsWith(suffix)
            ? target.Substring(0, target.Length - suffix.Length)
            : target;
    }

    internal class AdditionalParametersDocumentFilter : IDocumentFilter {
        public void Apply(OpenApiDocument openApiDoc, DocumentFilterContext context) {
            foreach (var schema in context.SchemaRepository.Schemas) {
                if (schema.Value.AdditionalProperties == null) {
                    schema.Value.AdditionalPropertiesAllowed = true;
                }
            }
        }
    }
}
