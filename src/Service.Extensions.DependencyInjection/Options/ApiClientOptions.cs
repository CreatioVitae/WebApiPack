using System.ComponentModel.DataAnnotations;

namespace Service.Extensions.DependencyInjection.Options;

public record ApiClientOptions {

    public int? TimeoutSeconds { get; init; }

    [Required]
    public string BaseAddress { get; init; } = null!;

    public static string GetDefaultSection(string apiClientName) =>
        $"{apiClientName}Options";
}
