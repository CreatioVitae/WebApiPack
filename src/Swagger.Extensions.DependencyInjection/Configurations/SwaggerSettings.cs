namespace Swagger.Extensions.DependencyInjection.Configurations {
    public class SwaggerSettings {
        public RouteTemplateSettings RouteTemplateSettings { get; set; }

        public OpenApiServerPathBaseSettings OpenApiServerPathBaseSettings { get; set; }
    }

    public class OpenApiServerPathBaseSettings {
        public bool IsOverridable { get; set; }
        public string PathBase { get; set; }
        public bool ForceHttps { get; set; }
    }

    public class RouteTemplateSettings {
        public bool IsOverridable { get; set; }
        public string RouteTemplatePrefix { get; set; }
    }
}
