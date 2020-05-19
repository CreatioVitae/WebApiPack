using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using Utf8Json.Resolvers;
using WebApiPack.ConstantValues;

namespace Microsoft.Extensions.DependencyInjection {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection CreateDefaultBuilder(this IServiceCollection serviceDescriptors) {
            serviceDescriptors
                .AddHttpContextAccessor()
                .AddControllers();

            serviceDescriptors
                .AddMvcCore(option => {
                    option.OutputFormatters.Clear();
                    option.OutputFormatters.Add(new Utf8Json.AspNetCoreMvcFormatter.JsonOutputFormatter(StandardResolver.ExcludeNullCamelCase));
                    option.Filters.Add(new ProducesAttribute("text/plain", "application/json", "text/json"));
                })
                .AddJsonOptions(option => option.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
                .AddCors(option => option.AddPolicy(CorsConstantValues.PolicyName, builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

            return serviceDescriptors;
        }
    }
}
