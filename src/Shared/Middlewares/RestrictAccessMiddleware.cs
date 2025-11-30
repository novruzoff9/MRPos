using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.ResultTypes;
using System.Text.Json;

namespace Shared.Middlewares;

public class RestrictAccessMiddleware(RequestDelegate next, ILogger<RestrictAccessMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var allowedPaths = new List<string>
        {
            "/connect/token",
            "/.well-known/openid-configuration",
        };

        if (allowedPaths.Any(path => context.Request.Path.StartsWithSegments(path)))
        {
            await next(context);
            return;
        }
        if (context.Request.ContentType?.StartsWith("application/grpc") == true)
        {
            await next(context);
            return;
        }


        if (!context.Request.Headers.TryGetValue("X-Correlation-ID", out Microsoft.Extensions.Primitives.StringValues value) || string.IsNullOrEmpty(value))
        {
            var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var userAgent = context.Request.Headers.UserAgent.ToString();
            var requestPath = context.Request.Path.ToString();
            var method = context.Request.Method;

            logger.LogWarning("Access denied for request: {Method} {Path} from IP: {ClientIp}, UserAgent: {UserAgent}. Missing or invalid X-Internal-Request header.",
                method, requestPath, clientIp, userAgent);

            logger.LogCritical($"{value}");

            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            var response = Response<string>.Fail("Icazə yoxdur", StatusCodes.Status403Forbidden);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));

            return;
        }
        await next(context);
    }
}