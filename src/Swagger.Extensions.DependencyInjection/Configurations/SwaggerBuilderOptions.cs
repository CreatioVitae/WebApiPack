namespace Swagger.Extensions.DependencyInjection.Configurations;

public class SwaggerBuilderOptions {
    public OpenApiServerOptions OpenApiServerOptions { get; init; } = null!;
    public GenerateOptions GenerateOptions { get; init; } = null!;
    public UiOptions UiOptions { get; init; } = null!;
}

public class OpenApiServerOptions {
    public bool ForceHttps { get; init; }
}

public class GenerateOptions {
    public AuthenticationOptions? AuthenticationOptions { get; init; }
    public bool XmlCommentsIncludingEnable { get; init; }
    public DocOptions DocOptions { get; init; } = null!;
}

public class AuthenticationOptions {
    public string AuthorizationType { get; init; } = null!;
}

public class DocOptions {
    public string Name { get; init; } = null!;
    public Info Info { get; init; } = null!;
}

public class Info {
    public string Title { get; init; } = null!;
    public string Version { get; init; } = null!;
}

public class UiOptions {
    public string Name { get; init; } = null!;
}
