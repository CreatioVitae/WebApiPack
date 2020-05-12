using Microsoft.AspNetCore.Mvc;
using Utf8Json.Resolvers;
using WebApiPack.ConstantValues;

namespace Microsoft.Extensions.DependencyInjection {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection CreateDefaultBuilder(this IServiceCollection serviceDescriptors) {
            serviceDescriptors
                .AddHttpContextAccessor()
                .AddMvcCore(option => {
                    option.OutputFormatters.Clear();
                    option.OutputFormatters.Add(new Utf8Json.AspNetCoreMvcFormatter.JsonOutputFormatter(StandardResolver.ExcludeNullCamelCase));
                    option.Filters.Add(new ProducesAttribute("text/plain", "application/json", "text/json"));
                })
                .AddApiExplorer()
                .AddAuthorization()
                .AddCors(option => option.AddPolicy(CorsConstantValues.PolicyName, builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()))
                .AddDataAnnotations()
                .AddFormatterMappings();

            return serviceDescriptors;
        }
    }
}
