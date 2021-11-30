using System;

namespace WebApiPack.Controllers.ApiResults;

public class ErrorResult {
    public ErrorResult() {
    }

    public ErrorResult(string? message, Exception? exception = default) {
        Message = message;
        Exception = exception;
    }

    public string? Message { get; }
    public Exception? Exception { get; }
}
