using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Utf8Json.Resolvers;
using WebApiPack.ConstantValues;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions {
    public static IServiceCollection CreateDefaultBuilder(this IServiceCollection serviceDescriptors, IEnumerable<Type>? filterTypesForGlobalEntry = null, bool useUtf8JsonAsOutputFormatter = true) {
        serviceDescriptors
            .AddHttpContextAccessor()
            .AddControllers();

        serviceDescriptors
            .AddMvcCore(option => {
                if (useUtf8JsonAsOutputFormatter) {
                    option.OutputFormatters.Clear();
                    option.OutputFormatters.Add(new Utf8Json.AspNetCoreMvcFormatter.JsonOutputFormatter(StandardResolver.ExcludeNullCamelCase));
                    option.Filters.Add(new ProducesAttribute("text/plain", "application/json", "text/json"));
                }

                if (filterTypesForGlobalEntry is IEnumerable<Type>) {
                    foreach (var filterTypeForGlobalEntry in filterTypesForGlobalEntry) {
                        option.Filters.Add(filterTypeForGlobalEntry);
                    }
                }
            })
            .AddJsonOptions(option => option.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
            .AddCors(option => option.AddPolicy(CorsConst.PolicyName, builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

        return serviceDescriptors;
    }
}
