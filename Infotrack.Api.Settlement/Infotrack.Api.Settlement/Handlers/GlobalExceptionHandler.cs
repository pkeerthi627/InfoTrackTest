using Infotrack.Api.Settlement.ApiProblemDetails;
using Infotrack.Api.Settlement.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Infotrack.Api.Settlement.Handlers
{
    [ExcludeFromCodeCoverage(Justification = "This is an Exception Handler Filter, hence not including for Coverage for now")]
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Exception Occured: {Message}", exception.Message);
            var contextAccessor = httpContext.RequestServices
                .GetRequiredService<IHttpContextAccessor>();

            var requestId = contextAccessor?
                .HttpContext?
                .GetRequestIdentifier() ?? string.Empty;

            var apiProblem = new ApiProblem("Internal Server Error", "Settlement Api : Application Error", "Internal Server Error", requestId);
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(apiProblem, cancellationToken);
            return true;
        }
    }
}
