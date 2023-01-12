using System;

namespace WebApiPack.Controllers.ApiResults;

public class ErrorResult {
    public ErrorResult() {
    }

    public ErrorResult(string? message, Exception? exception = default) {
        Message = message;

        Exception = ExceptionInfo.Create(exception);
    }

    public string? Message { get; }
    public ExceptionInfo? Exception { get; }
}

public record ExceptionInfo(string Value) {
    public static ExceptionInfo? Create(Exception? e) =>
        e is null
            ? default
            : new ExceptionInfo(e.ToString());
} 
