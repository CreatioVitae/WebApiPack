using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Service.Extensions.DependencyInjection.Markers;
using Service.Extensions.DependencyInjection.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddScopedServices<IMarker>(this IServiceCollection serviceDescriptors, Type[] types) {
        //Extend Points:Injection Process From Injection Marker.
        static IServiceCollection AddScopedServicesFromInjectionMarker<TInjectionMarker>(IServiceCollection serviceDescriptors, Type[] types) {
            foreach (var type in types.Where(c => c.GetInterfaces().Any(t => t == typeof(TInjectionMarker)))) {
                if (type.GetInterfaces().SingleOrDefault(i => i.Name == $"I{type.Name}") is { } specializedInterface) {
                    serviceDescriptors.AddScoped(specializedInterface, type);
                }
                else {
                    serviceDescriptors.AddScoped(type, type);
                }
            }

            return serviceDescriptors;
        }

        return AddScopedServicesFromInjectionMarker<IMarker>(serviceDescriptors, types);
    }

    public static IServiceCollection AddDefaultScopedServices(this IServiceCollection serviceDescriptors, Type[] types) =>
        serviceDescriptors.AddScopedServices<IService>(types).AddScopedServices<IRepository>(types).AddScopedServices<ICache>(types);

    public static IHttpClientBuilder AddApiClient<TClient>(this IServiceCollection services, Action<HttpClient> configureClient, PolicyHandleOptions? options = null) where TClient : class, IApiClient {
        options ??= PolicyHandleOptions.CreateDefault();

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(RetryPolicyOption option) {
            var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(option.MedianFirstRetryDelaySeconds), retryCount: option.RetryCount);

            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResultIf(option.ClientErrorStatusCodesRequiresRetry)
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

    static PolicyBuilder<HttpResponseMessage> OrResultIf(this PolicyBuilder<HttpResponseMessage> policyBuilder, IEnumerable<HttpStatusCode>? clientErrorStatusCodesRequiresRetry) =>
        clientErrorStatusCodesRequiresRetry is IEnumerable<HttpStatusCode> nonNullRequiresRetry && nonNullRequiresRetry.Any()
            ? policyBuilder.OrResult(msg => nonNullRequiresRetry.Contains(msg.StatusCode))
            : policyBuilder;
}
