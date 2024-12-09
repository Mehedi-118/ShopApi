using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using Serilog;

namespace eShop.Auth.Middleware;

public class GlobalExceptionHandlerMiddleware : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        Log.Error(exception: exception,
            "Exception occurred: {Message} Inner exception message: {InnerException} Request Url: {Url}",
            exception.Message, exception.InnerException?.Message, httpContext.Request.Path);
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Server error",
            Detail = "An error occurred while processing your request"
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;


        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}