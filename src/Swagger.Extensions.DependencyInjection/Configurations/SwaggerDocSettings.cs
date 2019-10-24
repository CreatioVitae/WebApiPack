namespace Swagger.Extensions.DependencyInjection.Configurations {
    public class SwaggerDocSettings {
        public string DocName { get; set; }

        public DocInfo Info { get; set; }
    }

    public class DocInfo {
        public string Title { get; set; }

        public string Version { get; set; }
    }
}
