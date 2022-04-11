using System.Collections.Generic;
using System.Net;

namespace Service.Extensions.DependencyInjection.Options;

public record PolicyHandleOptions {
    public RetryPolicyOption RetryPolicyOption { get; init; } = null!;

    public CircuitBreakerPolicyOption CircuitBreakerPolicyOption { get; init; } = null!;

    public static PolicyHandleOptions CreateDefault() =>
    new() {
        RetryPolicyOption = RetryPolicyOption.CreateDefault(),
        CircuitBreakerPolicyOption = CircuitBreakerPolicyOption.CreateDefault()
    };
}

public record RetryPolicyOption {
    public IEnumerable<HttpStatusCode>? ClientErrorStatusCodesRequiresRetry { get; init; }

    public int MedianFirstRetryDelaySeconds { get; init; }

    public int RetryCount { get; init; }

    public static RetryPolicyOption CreateDefault() =>
     new() { MedianFirstRetryDelaySeconds = 1, RetryCount = 3, ClientErrorStatusCodesRequiresRetry = null };
}

public record CircuitBreakerPolicyOption {
    public int HandledEventsAllowCountBeforeBreaking { get; init; }

    public int DurationOfBreakSeconds { get; init; }

    public static CircuitBreakerPolicyOption CreateDefault() =>
        new() { HandledEventsAllowCountBeforeBreaking = 5, DurationOfBreakSeconds = 30 };
}
