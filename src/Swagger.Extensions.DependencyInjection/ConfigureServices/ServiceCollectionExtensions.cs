using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using Swagger.Extensions.DependencyInjection.Configurations;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace Microsoft.Extensions.DependencyInjection {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddDefaultSwaggerService(this IServiceCollection serviceDescriptors, SwaggerGenSettings swaggerGenSettings) =>
            serviceDescriptors.AddSwaggerGen(c => {
                c.DefaultSwaggerDoc(swaggerGenSettings.SwaggerDocSettings);
                c.AddDefaultAuthentication(swaggerGenSettings);
                c.DefaultCustomOperationIds();
                c.DocumentFilter<AdditionalParametersDocumentFilter>();
            });

        public static IServiceCollection AddDefaultSwaggerService<TFilter>(this IServiceCollection serviceDescriptors, SwaggerGenSettings swaggerGenSettings) where TFilter : IOperationFilter =>
            serviceDescriptors.AddSwaggerGen(c => {
                c.DefaultSwaggerDoc(swaggerGenSettings.SwaggerDocSettings);
                c.AddDefaultAuthentication(swaggerGenSettings);
                c.DefaultCustomOperationIds();
                c.DocumentFilter<AdditionalParametersDocumentFilter>();

                c.OperationFilter<TFilter>();
            });

        internal static void AddDefaultAuthentication(this SwaggerGenOptions c, SwaggerGenSettings swaggerGenSettings) {
            if (swaggerGenSettings.AuthenticationSettings.AuthenticationIsEnable is false) {
                return;
            }

            switch (swaggerGenSettings.AuthenticationSettings.AuthorizationType) {
                case AuthorizationType.Basic:
                    c.AddBasicAuthentication(swaggerGenSettings);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        internal static void AddBasicAuthentication(this SwaggerGenOptions c, SwaggerGenSettings swaggerGenSettings) {
            c.AddSecurityDefinition(AuthorizationType.Basic, new OpenApiSecurityScheme {
                Name = HttpHeaderConsts.AuthorizationHeaderKey,
                Type = SecuritySchemeType.Http,
                Scheme = swaggerGenSettings.AuthenticationSettings.AuthorizationType,
                In = ParameterLocation.Header,
                Description = "Basic Authorization header using the Bearer scheme."
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = AuthorizationType.Basic
                        }
                    },
                    new string[] {}
                }
            });
        }

        internal static void DefaultSwaggerDoc(this SwaggerGenOptions c, SwaggerDocSettings swaggerDocSettings) =>
            c.SwaggerDoc(swaggerDocSettings.DocName, new OpenApiInfo { Title = swaggerDocSettings.Info.Title, Version = swaggerDocSettings.Info.Version });

        internal static void DefaultCustomOperationIds(this SwaggerGenOptions c) =>
            c.CustomOperationIds(apiDesc => apiDesc.TryGetMethodInfo(out var methodInfo) ? methodInfo.Name.RemoveFromEnd("Async") : null);

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
