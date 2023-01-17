using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
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

        return context.Response.WriteAsync(
            JsonSerializer.Serialize(
                new ErrorResult("システムエラーが発生しました。", exception),
                new JsonSerializerOptions {
                    Encoder = new NoEscapingJsonEncoder(),
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                }
            )
        );
    }
}
