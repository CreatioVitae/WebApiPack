using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Service.Extensions.DependencyInjection.Markers;
using Service.Extensions.DependencyInjection.Options;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddScopedServices<IMarker>(this IServiceCollection serviceDescriptors, Type[] types) {
        //Extend Points:Injection Process From Injection Marker.
        static IServiceCollection AddScopedServicesFromInjectionMarker<InjectionMarker>(IServiceCollection serviceDescriptors, Type[] types) {
            foreach (var type in types.Where(c => c.GetInterfaces().Any(t => t == typeof(InjectionMarker)))) {
                serviceDescriptors.AddScoped(type, type);
            }

            return serviceDescriptors;
        }

        return AddScopedServicesFromInjectionMarker<IMarker>(serviceDescriptors, types);
    }

    public static IServiceCollection AddDefaultScopedServices(this IServiceCollection serviceDescriptors, Type[] types) =>
        serviceDescriptors.AddScopedServices<IService>(types).AddScopedServices<IRepository>(types).AddScopedServices<ICache>(types);

    public static IServiceCollection AddRequestContext<TRequestContext>(this IServiceCollection serviceDescriptors, string timezoneId) where TRequestContext : class, IRequestContext, new() =>
        serviceDescriptors.AddScoped(serviceProvider => IRequestContext.CreateRequestContext<TRequestContext>(timezoneId));

    static readonly PolicyHandleOptions _defaultPolicyHandleOptions = new() {
        RetryPolicyOption = new() { MedianFirstRetryDelaySeconds = 1, RetryCount = 3 },
        CircuitBreakerPolicyOption = new() { HandledEventsAllowCountBeforeBreaking = 5, DurationOfBreakSeconds = 30 }
    };

    public static IHttpClientBuilder AddApiClient<TClient>(this IServiceCollection services, Action<HttpClient> configureClient, PolicyHandleOptions? options = null) where TClient : class, IApiClient {
        options ??= _defaultPolicyHandleOptions;

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(RetryPolicyOption option) {
            var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(option.MedianFirstRetryDelaySeconds), retryCount: option.RetryCount);

            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                .WaitAndRetryAsync(delay);
        }

        static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(CircuitBreakerPolicyOption option) =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(option.HandledEventsAllowCountBeforeBreaking, TimeSpan.FromSeconds(option.DurationOfBreakSeconds));

        return services
            .AddHttpClient<TClient>(configureClient)
            .SetHandlerLifetime(TimeSpan.FromMinutes(5)) // Todo: Optional
            .AddPolicyHandler(GetRetryPolicy(options.RetryPolicyOption))
            .AddPolicyHandler(GetCircuitBreakerPolicy(options.CircuitBreakerPolicyOption));
    }
}
