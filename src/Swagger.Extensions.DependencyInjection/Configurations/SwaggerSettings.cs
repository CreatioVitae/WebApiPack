namespace Swagger.Extensions.DependencyInjection.Configurations;

public class SwaggerSettings {
    public RouteTemplateSettings RouteTemplateSettings { get; set; } = null!;

    public OpenApiServerPathBaseSettings OpenApiServerPathBaseSettings { get; set; } = null!;
}

public class OpenApiServerPathBaseSettings {
    public bool IsOverridable { get; set; }
    public string PathBase { get; set; } = null!;
    public bool ForceHttps { get; set; }
}

public class RouteTemplateSettings {
    public bool IsOverridable { get; set; }
    public string RouteTemplatePrefix { get; set; } = null!;
}
