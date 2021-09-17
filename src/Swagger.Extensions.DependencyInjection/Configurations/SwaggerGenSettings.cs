namespace Swagger.Extensions.DependencyInjection.Configurations {
    public class SwaggerGenSettings {
        public AuthenticationSettings AuthenticationSettings { get; init; }

        public bool XmlCommentsIncludingEnable { get; init; }

        public SwaggerDocSettings SwaggerDocSettings { get; init; }
    }

    public class AuthenticationSettings {
        public bool AuthenticationEnable { get; init; }

        public string AuthorizationType { get; init; }
    }

    public class SwaggerDocSettings {
        public string DocName { get; init; }

        public DocInfo Info { get; init; }
    }

    public class DocInfo {
        public string Title { get; init; }

        public string Version { get; init; }
    }
}
