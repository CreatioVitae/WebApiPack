using Utf8Json.Resolvers;
using WebApiPack.ConstantValues;

namespace Microsoft.Extensions.DependencyInjection {
    public static class ServiceCollectionExtensions {
        public static IMvcCoreBuilder CreateDefaultBuilder(this IServiceCollection serviceDescriptors) {
            return serviceDescriptors
                .AddMvcCore(option => {
                    option.OutputFormatters.Clear();
                    option.OutputFormatters.Add(new Utf8Json.AspNetCoreMvcFormatter.JsonOutputFormatter(StandardResolver.ExcludeNullCamelCase));
                    option.InputFormatters.Clear();
                    option.InputFormatters.Add(new Utf8Json.AspNetCoreMvcFormatter.JsonInputFormatter(StandardResolver.ExcludeNullCamelCase));

                })
                .AddApiExplorer()
                .AddAuthorization()
                .AddCors(option => { option.AddPolicy(CorsConstantValues.PolicyName, builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); })
                .AddDataAnnotations()
                .AddFormatterMappings();
        }
    }
}