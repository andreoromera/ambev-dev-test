using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.Dev.Test.Api.Handlers
{
    public class CustomExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = "An error has occurred",
                Detail = exception.Message,
                Type = "https://httpstatuses.com/400",
                Status = StatusCodes.Status400BadRequest
            }, cancellationToken: cancellationToken);

            return true;
        }
    }
}
