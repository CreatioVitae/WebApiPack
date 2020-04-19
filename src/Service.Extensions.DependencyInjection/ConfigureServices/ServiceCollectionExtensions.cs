using Service.Extensions.DependencyInjection.Markers;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection {
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
            serviceDescriptors.AddScopedServices<IService>(types).AddScopedServices<IRepository>(types);
    }
}
