namespace Swagger.Extensions.DependencyInjection.Configurations;

public class SwaggerUiSettings {
    public string Url { get; set; } = null!;
    public string Name { get; set; } = null!;
    public RoutePrefixSettings RoutePrefixSettings { get; set; } = null!;
}

public class RoutePrefixSettings {
    public bool IsOverridable { get; set; }
    public string RoutePrefix { get; set; } = null!;
}
