using Utf8Json.Resolvers;
using WebApiPack.ConstantValues;

namespace Microsoft.Extensions.DependencyInjection {
    public static class ServiceCollectionExtensions {
        public static IMvcCoreBuilder CreateDefaultBuilder(this IServiceCollection serviceDescriptors) =>
            serviceDescriptors
                .AddHttpContextAccessor()
                .AddMvcCore(option => {
                    option.OutputFormatters.Clear();
                    option.OutputFormatters.Add(new Utf8Json.AspNetCoreMvcFormatter.JsonOutputFormatter(StandardResolver.ExcludeNullCamelCase));
                })
                .AddApiExplorer()
                .AddAuthorization()
                .AddCors(option => { option.AddPolicy(CorsConstantValues.PolicyName, builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); })
                .AddDataAnnotations()
                .AddFormatterMappings();
    }
}
