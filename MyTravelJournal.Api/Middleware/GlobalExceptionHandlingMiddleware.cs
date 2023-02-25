using MyTravelJournal.Api.Exceptions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace MyTravelJournal.Api.Middleware;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }


    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {

        var stackTrace = ex.StackTrace;
        var message = ex.Message;

        var exceptionResult = JsonSerializer.Serialize(new
        {
            error = message,
            stackTrace
        });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = GetStatusCode(ex);
        
        return context.Response.WriteAsync(exceptionResult);
    }

    private static int GetStatusCode(Exception ex)
    {
        return ex.GetType() switch
        {
            { } t when t == typeof(NotFoundException) => StatusCodes.Status404NotFound,
            { } t when t == typeof(ValidationException) => StatusCodes.Status400BadRequest,
            { } t when t == typeof(ConflictException) => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}