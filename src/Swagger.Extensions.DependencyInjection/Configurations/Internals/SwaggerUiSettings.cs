using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Swagger.Extensions.DependencyInjection.Configurations;

internal class SwaggerUiSettings {

    internal string Url { get; set; } = null!;
    internal string Name { get; set; } = null!;
    internal RoutePrefixSettings RoutePrefixSettings { get; set; } = null!;

    internal SwaggerUiSettings(ConfigureSettings? configureSettings, SwaggerBuilderOptions swaggerBuilderOptions) {

        var urlFragment = new PathString(SwaggerUiSettingsConsts.SwaggerPathFragment)
            .Add($"/{swaggerBuilderOptions.GenerateOptions.DocOptions.Name}")
            .Add("/swagger.json");

        Url = (configureSettings is { PathBase: not null })
            ? configureSettings.PathBase.Value.Add(urlFragment).ToString()
            : urlFragment.ToString();

        Name = swaggerBuilderOptions.UiOptions.Name;

        RoutePrefixSettings = new RoutePrefixSettings(configureSettings);
    }
}

internal static class SwaggerUiSettingsConsts {
    internal static readonly string SwaggerPathFragment = "/swagger";
}

internal class RoutePrefixSettings {
    internal bool IsOverridable { get; set; }
    internal string RoutePrefix { get; set; } = null!;

    internal RoutePrefixSettings(ConfigureSettings? configureSettings) => (IsOverridable, RoutePrefix) = (configureSettings is { PathBase: not null })
        ? (true, configureSettings.PathBase.Value.Add(SwaggerUiSettingsConsts.SwaggerPathFragment).WithoutLeadingSlash(false))
        : (false, string.Empty);
}
