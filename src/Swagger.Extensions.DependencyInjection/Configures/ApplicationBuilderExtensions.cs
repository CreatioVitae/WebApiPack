using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Swagger.Extensions.DependencyInjection.Configurations;

namespace Microsoft.AspNetCore.Builder {
    public static class ApplicationBuilderExtensions {
        public static IApplicationBuilder UseDefaultSwaggerBuilder(this IApplicationBuilder app, SwaggerSettings swaggerSettings, SwaggerUiSettings swaggerUiSettings, string environmentName) {
            static IApplicationBuilder UseDefaultSwaggerBuilderLocal(IApplicationBuilder app, SwaggerSettings swaggerSettings, SwaggerUiSettings swaggerUiSettings) =>
                app
                    .UseSwagger(option => {
                        if (swaggerSettings.RouteTemplateSettings.IsOverridable) {
                            option.RouteTemplate = swaggerSettings.RouteTemplateSettings.RouteTemplate;
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
}
