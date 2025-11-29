using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Exceptions.Common;
using Shared.ResultTypes;
using System.Net.Http;
using System.Text.Json;

namespace Shared.Middlewares;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        logger.LogDebug("ExceptionHandlingMiddleware invoked.");
        try
        {
            await next(context);
        }
        catch (BaseException ex)
        {
            if (context.Response.HasStarted)
            {
                logger.LogWarning("Response has already started, cannot write error response.");
                return;
            }
            LogError(ex);
            await HandleExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            if (context.Response.HasStarted)
            {
                logger.LogWarning("Response has already started, cannot write error response.");
                return;
            }
            LogError(ex);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        if (context.Response.HasStarted)
            return Task.CompletedTask;
        var statusCode = (ex as BaseException)?.StatusCode ?? 500;

        var response = Response<string>.Fail($"{ex.Message}", statusCode);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private void LogError(Exception ex)
    {
        var st = new System.Diagnostics.StackTrace(ex, true);
        var frame = st.GetFrames()?.FirstOrDefault(f => f.GetFileLineNumber() > 0);
        var fileName = frame?.GetFileName();
        var line = frame?.GetFileLineNumber();
        var method = frame?.GetMethod()?.Name;
        logger.LogError(
            $"Error: {ex.Message}\nFile: {fileName} | Line: {line} | Time: {DateTime.Now:HH:mm:ss} "
        );
    }
}

