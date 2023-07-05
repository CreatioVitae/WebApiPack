using Microsoft.AspNetCore.Hosting;
using Microsoft.OpenApi.Any;
using WebPack.CoreLib.HttpHeaders.RequestedDatetimeOverrides;

// ReSharper disable once CheckNamespace
namespace Microsoft.OpenApi.Models;
public static class OpenApiOperationExtensions {
    public static void AddRequestedDatetimeOverrideOperationParameters(this OpenApiOperation operation, string timezoneId, string requestedDatetimeFormat) {
        if (DefaultWebEnvironment.WebApps.IsNotDevelopment()) {
            return;
        }

        operation.Parameters.Add(new OpenApiParameter {
            Name = HeaderKeys.ForceOverrideRequestedDatetime,
            In = ParameterLocation.Header,
            AllowEmptyValue = true,
            Required = false,
            Schema = new OpenApiSchema { Type = "boolean" }
        });

        operation.Parameters.Add(new OpenApiParameter {
            Name = HeaderKeys.OverrideRequestedDatetime,
            In = ParameterLocation.Header,
            AllowEmptyValue = true,
            Required = false,
            Schema = new OpenApiSchema { Type = "string", Example = new OpenApiString(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(timezoneId)).ToString(requestedDatetimeFormat)) }
        });
    }
}
