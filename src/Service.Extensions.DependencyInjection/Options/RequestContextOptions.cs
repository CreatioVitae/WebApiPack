using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.Extensions.DependencyInjection.Options;

//Todo:C#11でnull!除去予定
public record RequestContextOptions {
    [Required]
    public UserAgentForceOptionInDevelopment UserAgentForceOptionInDevelopment { get; init; } = null!;

    public bool CanForceOverrideUserAgent() =>
        DefaultEnvironment.GetEnvironmentName() is DefaultEnvironmentNames.Development && UserAgentForceOptionInDevelopment?.ForceOverride is true;

    public string GetUserAgent(HttpContext httpContext) =>
        CanForceOverrideUserAgent() ? UserAgentForceOptionInDevelopment.OverrideValue : httpContext.Request.Headers.UserAgent;
}

public record UserAgentForceOptionInDevelopment {
    public bool ForceOverride { get; init; }

    [Required]
    public string OverrideValue { get; init; } = null!;
}
