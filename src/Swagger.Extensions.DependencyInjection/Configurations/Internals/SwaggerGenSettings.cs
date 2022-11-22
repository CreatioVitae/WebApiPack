using Microsoft.AspNetCore.Builder;

// ReSharper disable once CheckNamespace
namespace Swagger.Extensions.DependencyInjection.Configurations;

internal class SwaggerGenSettings {

    internal AuthenticationSettings AuthenticationSettings { get; }

    internal bool XmlCommentsIncludingEnable { get; }

    internal SwaggerDocSettings SwaggerDocSettings { get; }

    internal SwaggerGenSettings(ConfigureSettings? configureSettings, SwaggerBuilderOptions swaggerBuilderOptions) {
        AuthenticationSettings = new(configureSettings, swaggerBuilderOptions);
        XmlCommentsIncludingEnable = swaggerBuilderOptions.GenerateOptions.XmlCommentsIncludingEnable;
        SwaggerDocSettings = new(swaggerBuilderOptions);
    }
}

internal class AuthenticationSettings {
    internal bool AuthenticationEnable { get; }

    internal string AuthorizationType { get; }

    internal AuthenticationSettings(ConfigureSettings? configureSettings, SwaggerBuilderOptions swaggerBuilderOptions) {
        AuthenticationEnable = configureSettings?.AuthenticationEnable ?? false;
        AuthorizationType = swaggerBuilderOptions.GenerateOptions.AuthenticationOptions?.AuthorizationType ?? string.Empty;
    }
}

internal class SwaggerDocSettings {
    internal string DocName { get; }

    internal DocInfo Info { get; }

    internal SwaggerDocSettings(SwaggerBuilderOptions swaggerBuilderOptions) {
        DocName = swaggerBuilderOptions.GenerateOptions.DocOptions.Name;
        Info = new(swaggerBuilderOptions);
    }
}

internal class DocInfo {
    internal string Title { get; }

    internal string Version { get; }

    internal DocInfo(SwaggerBuilderOptions swaggerBuilderOptions) {
        Title = swaggerBuilderOptions.GenerateOptions.DocOptions.Info.Title;
        Version = swaggerBuilderOptions.GenerateOptions.DocOptions.Info.Version;
    }
}
