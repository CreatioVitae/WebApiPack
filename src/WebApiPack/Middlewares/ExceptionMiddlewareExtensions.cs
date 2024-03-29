using WebApiPack.Middlewares;

namespace Microsoft.AspNetCore.Builder;

public static class ExceptionMiddlewareExtensions {
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app) =>
        app.UseMiddleware<ExceptionMiddleware>();
}
