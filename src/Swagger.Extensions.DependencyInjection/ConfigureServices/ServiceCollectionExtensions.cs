using Swagger.Extensions.DependencyInjection.Configurations;
using Swashbuckle.AspNetCore.Swagger;

namespace Microsoft.Extensions.DependencyInjection {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddDefaultSwaggerService(this IServiceCollection serviceDescriptors, SwaggerDocSettings swaggerDocSettings) {
            return serviceDescriptors.AddSwaggerGen(c => { c.SwaggerDoc(swaggerDocSettings.DocName, new Info() { Title = swaggerDocSettings.Info.Title, Version = swaggerDocSettings.Info.Version });});
        }
    }
}