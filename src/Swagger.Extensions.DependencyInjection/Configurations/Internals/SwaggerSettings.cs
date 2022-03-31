using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Swagger.Extensions.DependencyInjection.Configurations;

internal class SwaggerSettings {
    internal RouteTemplateSettings RouteTemplateSettings { get; set; } = null!;

    internal OpenApiServerPathBaseSettings OpenApiServerPathBaseSettings { get; set; } = null!;

    internal SwaggerSettings(ConfigureSettings? configureSettings, SwaggerBuilderOptions swaggerBuilderOptions) {
        RouteTemplateSettings = new(configureSettings);
        OpenApiServerPathBaseSettings = new(configureSettings, swaggerBuilderOptions);
    }
}

internal class OpenApiServerPathBaseSettings {
    internal bool IsOverridable { get; }

    internal string PathBase { get; }

    internal bool ForceHttps { get; }

    internal OpenApiServerPathBaseSettings(ConfigureSettings? configureSettings, SwaggerBuilderOptions swaggerBuilderOptions) =>
        (IsOverridable, PathBase, ForceHttps) = (configureSettings is { PathBase: not null })
            ? (true, configureSettings.PathBase.Value.ToString(), swaggerBuilderOptions.OpenApiServerOptions.ForceHttps)
            : (false, string.Empty, swaggerBuilderOptions.OpenApiServerOptions.ForceHttps);
}

internal class RouteTemplateSettings {
    internal bool IsOverridable { get; }

    /// <summary>
    /// xxx/yyy/zzz/
    /// </summary>
    internal string? RouteTemplatePrefix { get; }

    internal RouteTemplateSettings(ConfigureSettings? configureSettings) =>
        (IsOverridable, RouteTemplatePrefix) = (configureSettings is { PathBase: not null })
            ? (true, configureSettings.PathBase.Value.WithoutLeadingSlash(true))
            : (false, string.Empty);
}
