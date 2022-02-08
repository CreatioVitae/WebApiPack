namespace Swagger.Extensions.DependencyInjection.Configurations;

public class SwaggerGenSettings {
    public AuthenticationSettings AuthenticationSettings { get; init; } = null!;

    public bool XmlCommentsIncludingEnable { get; init; }

    public SwaggerDocSettings SwaggerDocSettings { get; init; } = null!;
}

public class AuthenticationSettings {
    public bool AuthenticationEnable { get; init; }

    public string AuthorizationType { get; init; } = null!;
}

public class SwaggerDocSettings {
    public string DocName { get; init; } = null!;

    public DocInfo Info { get; init; } = null!;
}

public class DocInfo {
    public string Title { get; init; } = null!;

    public string Version { get; init; } = null!;
}
