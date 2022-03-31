using BclExtensionPack.CoreLib;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swagger.Extensions.DependencyInjection.Configurations;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Builder;

public static class ApplicationBuilderExtensions {
    public static IApplicationBuilder UseDefaultSwaggerBuilder(this IApplicationBuilder app, IConfiguration configuration, string environmentName) {
        static IApplicationBuilder UseDefaultSwaggerBuilderLocal(IApplicationBuilder app, IConfiguration configuration) {
            var builderOptions = configuration.GetSwaggerBuilderOptions();
            var configureSettings = configuration.GetConfigureSettings();
            var (swaggerSettings, swaggerUiSettings) = (new SwaggerSettings(configureSettings, builderOptions), new SwaggerUiSettings(configureSettings, builderOptions));

            return app
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
        }

        return environmentName switch {
            DefaultEnvironmentNames.Development => UseDefaultSwaggerBuilderLocal(app, configuration),
            DefaultEnvironmentNames.DevelopmentRemote => UseDefaultSwaggerBuilderLocal(app, configuration),
            DefaultEnvironmentNames.Staging => app,
            DefaultEnvironmentNames.Production => app,
            _ => app
        };

    }

    public static SwaggerBuilderOptions GetSwaggerBuilderOptions(this IConfiguration configuration) =>
        configuration.GetSection(nameof(SwaggerBuilderOptions)).GetAvailable<SwaggerBuilderOptions>();
}
