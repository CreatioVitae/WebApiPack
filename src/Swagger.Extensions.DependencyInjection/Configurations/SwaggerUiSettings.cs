namespace Swagger.Extensions.DependencyInjection.Configurations;

public class SwaggerUiSettings {
    public string Url { get; set; }
    public string Name { get; set; }
    public RoutePrefixSettings RoutePrefixSettings { get; set; }
}

public class RoutePrefixSettings {
    public bool IsOverridable { get; set; }
    public string RoutePrefix { get; set; }
}
