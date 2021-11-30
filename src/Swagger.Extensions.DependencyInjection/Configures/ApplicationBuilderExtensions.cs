using BclExtensionPack.CoreLib;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Swagger.Extensions.DependencyInjection.Configurations;
using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Builder;

public static class ApplicationBuilderExtensions {
    public static IApplicationBuilder UseDefaultSwaggerBuilder(this IApplicationBuilder app, SwaggerSettings swaggerSettings, SwaggerUiSettings swaggerUiSettings, string environmentName) {
        static IApplicationBuilder UseDefaultSwaggerBuilderLocal(IApplicationBuilder app, SwaggerSettings swaggerSettings, SwaggerUiSettings swaggerUiSettings) =>
            app
                .UseSwagger(option => {
                    if (swaggerSettings.RouteTemplateSettings.IsOverridable) {
                        option.RouteTemplate = $"{swaggerSettings.RouteTemplateSettings.RouteTemplatePrefix}{option.RouteTemplate}";
                    }

                    if (swaggerSettings.OpenApiServerPathBaseSettings.IsOverridable) {
                        option.PreSerializeFilters.Add((swaggerDoc, httpRequest) => {
                            static string GetSchemeName(HttpRequest httpRequest, bool forceHttps) =>
                                forceHttps ? Uri.UriSchemeHttps : httpRequest.Scheme;

                            static UriBuilder CreateUriBuilder(HttpRequest httpRequest, string schemeName) =>
                                httpRequest.Host.Port switch {
                                    null => new UriBuilder(schemeName, httpRequest.Host.Host),
                                    _ => new UriBuilder(schemeName, httpRequest.Host.Host, httpRequest.Host.Port.Value)
                                };

                            swaggerDoc.Servers = new List<OpenApiServer> {
                                    new() { Url = CreateUriBuilder(
                                        httpRequest,
                                        GetSchemeName(httpRequest,swaggerSettings.OpenApiServerPathBaseSettings.ForceHttps)
                                    ).AppendPath(swaggerSettings.OpenApiServerPathBaseSettings.PathBase).ToString() }
                            };
                        });
                    }
                })
                .UseSwaggerUI(option => {
                    option.SwaggerEndpoint(swaggerUiSettings.Url, swaggerUiSettings.Name);
                    if (swaggerUiSettings.RoutePrefixSettings.IsOverridable) {
                        option.RoutePrefix = swaggerUiSettings.RoutePrefixSettings.RoutePrefix;
                    }
                });

        return environmentName switch {
            DefaultEnvironmentNames.Development => UseDefaultSwaggerBuilderLocal(app, swaggerSettings, swaggerUiSettings),
            DefaultEnvironmentNames.DevelopmentRemote => UseDefaultSwaggerBuilderLocal(app, swaggerSettings, swaggerUiSettings),
            DefaultEnvironmentNames.Staging => app,
            DefaultEnvironmentNames.Production => app,
            _ => app
        };

    }

    public static SwaggerSettings GetSwaggerSettings(this IConfiguration configuration) =>
        configuration.GetSection(nameof(SwaggerSettings)).Get<SwaggerSettings>();

    public static SwaggerUiSettings GetSwaggerUiSettings(this IConfiguration configuration) =>
        configuration.GetSection(nameof(SwaggerUiSettings)).Get<SwaggerUiSettings>();
}
