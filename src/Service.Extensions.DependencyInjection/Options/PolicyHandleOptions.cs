namespace Service.Extensions.DependencyInjection.Options;

public record PolicyHandleOptions {
    public RetryPolicyOption RetryPolicyOption { get; init; } = null!;

    public CircuitBreakerPolicyOption CircuitBreakerPolicyOption { get; init; } = null!;
}

public record RetryPolicyOption {
    public int MedianFirstRetryDelaySeconds { get; init; }

    public int RetryCount { get; init; }
}

public record CircuitBreakerPolicyOption {
    public int HandledEventsAllowCountBeforeBreaking { get; init; }

    public int DurationOfBreakSeconds { get; init; }
}
