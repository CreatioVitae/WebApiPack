using Microsoft.OpenApi.Models;
using Swagger.Extensions.DependencyInjection.Configurations;

namespace Microsoft.Extensions.DependencyInjection {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddDefaultSwaggerService(this IServiceCollection serviceDescriptors, SwaggerDocSettings swaggerDocSettings) =>
            serviceDescriptors.AddSwaggerGen(c => c.SwaggerDoc(swaggerDocSettings.DocName, new OpenApiInfo { Title = swaggerDocSettings.Info.Title, Version = swaggerDocSettings.Info.Version }));
    }
}
