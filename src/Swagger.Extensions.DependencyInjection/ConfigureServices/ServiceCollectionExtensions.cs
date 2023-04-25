using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Swagger.Extensions.DependencyInjection.Configurations;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IO;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddDefaultSwaggerService(this IServiceCollection serviceDescriptors, IConfiguration configuration, Assembly? executingAssembly = null) {
        var swaggerGenSettings = new SwaggerGenSettings(configuration.GetConfigureSettings(), configuration.GetSwaggerBuilderOptions());

        return serviceDescriptors.AddSwaggerGen(c => {
            c.AddDefaultSwaggerDoc(swaggerGenSettings.SwaggerDocSettings);
            c.AddDefaultAuthentication(swaggerGenSettings);
            c.AddDefaultCustomOperationIds();

            if (executingAssembly is not null) {
                c.AddXmlCommentsIncluding(swaggerGenSettings, executingAssembly);
            }

            c.DocumentFilter<AdditionalParametersDocumentFilter>();
        });
    }

    public static IServiceCollection AddDefaultSwaggerService<TFilter>(this IServiceCollection serviceDescriptors, IConfiguration configuration, Assembly? executingAssembly = null) where TFilter : IOperationFilter {
        var swaggerGenSettings = new SwaggerGenSettings(configuration.GetConfigureSettings(), configuration.GetSwaggerBuilderOptions());

        return serviceDescriptors.AddSwaggerGen(c => {
            c.AddDefaultSwaggerDoc(swaggerGenSettings.SwaggerDocSettings);
            c.AddDefaultAuthentication(swaggerGenSettings);
            c.AddDefaultCustomOperationIds();

            if (executingAssembly is not null) {
                c.AddXmlCommentsIncluding(swaggerGenSettings, executingAssembly);
            }

            c.DocumentFilter<AdditionalParametersDocumentFilter>();
            c.OperationFilter<TFilter>();
        });
    }

    internal static void AddDefaultAuthentication(this SwaggerGenOptions c, SwaggerGenSettings swaggerGenSettings) {
        if (swaggerGenSettings.AuthenticationSettings.AuthenticationEnable is false) {
            return;
        }

        switch (swaggerGenSettings.AuthenticationSettings.AuthorizationType) {
            case AuthorizationType.Basic:
                c.AddBasicAuthentication();
                break;
            case AuthorizationType.Bearer:
                c.AddTokenAuthentication();
                break;
            default:
                throw new NotImplementedException();
        }
    }

    internal static void AddBasicAuthentication(this SwaggerGenOptions c) {
        c.AddSecurityDefinition(AuthorizationType.Basic, new OpenApiSecurityScheme {
            Name = HttpHeaderConsts.AuthorizationHeaderKey,
            Type = SecuritySchemeType.Http,
            Scheme = AuthorizationType.Basic,
            In = ParameterLocation.Header,
            Description = "Authorization header using the basic scheme."
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = AuthorizationType.Basic
                        }
                    },
                    Array.Empty<string>()
                }
            });
    }

    internal static void AddTokenAuthentication(this SwaggerGenOptions c) {
        c.AddSecurityDefinition(AuthorizationType.Bearer, new OpenApiSecurityScheme {
            Name = HttpHeaderConsts.AuthorizationHeaderKey,
            Type = SecuritySchemeType.Http,
            Scheme = AuthorizationType.Bearer,
            In = ParameterLocation.Header,
            Description = "Authorization header using the bearer scheme."
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                        Id = AuthorizationType.Bearer
                    }
                },
                Array.Empty<string>()
            }
        });
    }

    internal static void AddXmlCommentsIncluding(this SwaggerGenOptions c, SwaggerGenSettings swaggerGenSettings, Assembly executingAssembly) {
        if (swaggerGenSettings.XmlCommentsIncludingEnable is false) {
            return;
        }

        var xmlFile = $"{executingAssembly.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    }
    internal static void AddDefaultSwaggerDoc(this SwaggerGenOptions c, SwaggerDocSettings swaggerDocSettings) =>
        c.SwaggerDoc(swaggerDocSettings.DocName, new OpenApiInfo { Title = swaggerDocSettings.Info.Title, Version = swaggerDocSettings.Info.Version });

    internal static void AddDefaultCustomOperationIds(this SwaggerGenOptions c) =>
        c.CustomOperationIds(apiDesc => apiDesc.TryGetMethodInfo(out var methodInfo) ? methodInfo.Name.RemoveFromEnd("Async") : null);

    internal static string RemoveFromEnd(this string target, string suffix) =>
        target.EndsWith(suffix)
        ? target[..^suffix.Length]
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
