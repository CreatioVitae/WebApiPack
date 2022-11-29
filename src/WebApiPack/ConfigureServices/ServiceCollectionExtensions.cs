using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using WebApiPack.ConstantValues;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions {
    public static IServiceCollection CreateDefaultBuilder(this IServiceCollection serviceDescriptors, IEnumerable<Type>? filterTypesForGlobalEntry = null) {
        serviceDescriptors
            .AddHttpContextAccessor()
            .AddControllers();

        serviceDescriptors
            .AddMvcCore(option => {
                if (filterTypesForGlobalEntry is not null) {
                    foreach (var filterTypeForGlobalEntry in filterTypesForGlobalEntry) {
                        option.Filters.Add(filterTypeForGlobalEntry);
                    }
                }
            })
            .AddJsonOptions(option => {
                option.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                option.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            })
            .AddCors(option => option.AddPolicy(CorsConst.PolicyName, builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

        return serviceDescriptors;
    }
}
