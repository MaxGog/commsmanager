using System.Text.Json;
using CommsManager.Application.Exceptions;

namespace CommsManager.API.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unhandled exception occurred");
            await HandleExceptionAsync(context, exception);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        object response;

        switch (exception)
        {
            case ValidationException validationException:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response = new { message = exception.Message, errors = validationException.Errors };
                break;

            case NotFoundException:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                response = new { message = exception.Message };
                break;

            case BusinessException:
                context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                response = new { message = exception.Message };
                break;

            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                response = new { message = "An internal server error occurred" };
                break;
        }

        return context.Response.WriteAsJsonAsync(response);
    }
}
