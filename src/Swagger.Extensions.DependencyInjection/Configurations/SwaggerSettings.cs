namespace Swagger.Extensions.DependencyInjection.Configurations {
    public class SwaggerSettings {
        public RouteTemplateSettings RouteTemplateSettings { get; set; }
    }

    public class RouteTemplateSettings {
        public bool IsOverridable { get; set; }
        public string RouteTemplate { get; set; }
    }
}
