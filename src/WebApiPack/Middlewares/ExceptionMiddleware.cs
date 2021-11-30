using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Threading.Tasks;
using Utf8Json.Resolvers;
using WebApiPack.Controllers.ApiResults;

namespace WebApiPack.Middlewares;

public class ExceptionMiddleware {
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext httpContext) {
        try {
            await _next(httpContext);
        }
        catch (Exception ex) {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception) {
        //Todo:HttpException Handling...

        Log.Fatal(exception, $"{nameof(HandleExceptionAsync)}", new { originalPath = context.Request.Path, RequestHeaders = context.Request.Headers });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        return context.Response.WriteAsync(Utf8Json.JsonSerializer.ToJsonString(new ErrorResult("システムエラーが発生しました。", exception), StandardResolver.ExcludeNullCamelCase));
    }
}
