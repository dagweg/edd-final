using HouseRentalSystem.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HouseRentalSystem.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (UnauthorizedAccessException ex)
        {
            var problemDetails = new ProblemDetails
            {
                Title = "Unauthorized",
                Detail = ex.Message,
                Status = StatusCodes.Status401Unauthorized,
                Extensions = { { "traceId", context.TraceIdentifier } },
            };

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            await context.Response.WriteAsJsonAsync(problemDetails);
            LogError(context, ex);
        }
        catch (AlreadyExistsException ex)
        {
            var problemDetails = new ProblemDetails
            {
                Title = "Invalid Operation",
                Detail = ex.Message,
                Status = StatusCodes.Status409Conflict,
                Extensions = { { "traceId", context.TraceIdentifier } },
            };

            context.Response.StatusCode = StatusCodes.Status409Conflict;

            await context.Response.WriteAsJsonAsync(problemDetails);
            LogError(context, ex);
        }
        catch (KeyNotFoundException ex)
        {
            var problemDetails = new ProblemDetails
            {
                Title = "Resource not found",
                Detail = ex.Message,
                Status = StatusCodes.Status404NotFound,
                Extensions = { { "traceId", context.TraceIdentifier } },
            };

            context.Response.StatusCode = StatusCodes.Status404NotFound;

            await context.Response.WriteAsJsonAsync(problemDetails);
            LogError(context, ex);
        }
        catch (ArgumentException ex)
        {
            var problemDetails = new ProblemDetails
            {
                Title = "Invalid Argument",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest,
                Extensions = { { "traceId", context.TraceIdentifier } },
            };

            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            await context.Response.WriteAsJsonAsync(problemDetails);
            LogError(context, ex);
        }
        catch (Exception ex)
        {
            var problemDetails = new ProblemDetails
            {
                Title = "InternalServer Error",
                Detail = "An internal server error has occured",
                Extensions = { { "traceId", context.TraceIdentifier } },
            };

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsJsonAsync(problemDetails);

            LogError(context, ex);
        }
    }

    public static void LogError(HttpContext context, Exception ex)
    {
        Console.Error.WriteLine("An internal server error has occured");
        Console.Error.WriteLine("TraceId: " + context.TraceIdentifier);
        Console.Error.WriteLine("Exception: " + ex);
    }
}
