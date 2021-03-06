using Microsoft.AspNetCore.Hosting;
using Swagger.Extensions.DependencyInjection.Configurations;
using System;

namespace Microsoft.AspNetCore.Builder {
    public static class ApplicationBuilderExtensions {
        public static IApplicationBuilder UseDefaultSwaggerBuilder(this IApplicationBuilder app, SwaggerUiSettings swaggerUiSettings, string environmentName) =>
            environmentName switch
            {
                DefaultEnvironmentNames.Development => app.UseSwagger().UseSwaggerUI(option => { option.SwaggerEndpoint(swaggerUiSettings.Url, swaggerUiSettings.Name); }),
                DefaultEnvironmentNames.DevelopmentRemote => app.UseSwagger().UseSwaggerUI(option => { option.SwaggerEndpoint(swaggerUiSettings.Url, swaggerUiSettings.Name); }),
                DefaultEnvironmentNames.Staging => app,
                DefaultEnvironmentNames.Production => app,
                _ => throw new ArgumentException(nameof(environmentName))
            };
    }
}
