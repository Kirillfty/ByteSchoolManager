using ByteSchoolManager.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ByteSchoolManager;

public class ProblemDetailsExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = exception switch
        {
            NotFoundException => 404,
            BadRequestException => 400,
            _ => 500
        };

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = new ProblemDetails
                {
                    Title = exception switch
                    {
                        NotFoundException => "Not Found",
                        BadRequestException => "Bad Request",
                        _ => "Inner Exception"
                    },
                    Type = exception.GetType().Name,
                    Detail = exception.Message,
                    Status = httpContext.Response.StatusCode
                }
            }
        );
    }
}