using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Acme.Center.Platform.Shared.Infrastructure.Pipeline.Middleware.Components;

/// <summary>
///     Global Exception Handling Middleware
/// </summary>
/// <remarks>
///     This middleware catches all unhandled exceptions and returns a Problem Details response.
/// </remarks>
public class GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
{
    /**
     * <summary>
     *     Invoke the middleware
     * </summary>
     * <param name="context">The http context</param>
     * <returns>A task</returns>
     */
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    /**
     * <summary>
     *     Handle the exception
     * </summary>
     * <param name="context">The http context</param>
     * <param name="exception">The exception</param>
     * <returns>A task</returns>
     */
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An unexpected error occurred",
            Detail = exception.Message,
            Instance = context.Request.Path
        };

        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var result = JsonSerializer.Serialize(problemDetails, jsonOptions);

        await context.Response.WriteAsync(result);
    }
}
