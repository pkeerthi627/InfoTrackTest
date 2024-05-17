using System.Diagnostics.CodeAnalysis;

namespace Infotrack.Api.Settlement.Infrastructure
{
    [ExcludeFromCodeCoverage(Justification = "This is an extension to get Request-Id from header, hence not including for coverage for now- To be done when considering extending")]
    public static class HttpContextExtension
    {
        public static string? GetRequestIdentifier(this HttpContext context)
        {
            if (context == null) return null;
            var requestId = context.Items["request-id"]?.ToString();
            if (!string.IsNullOrEmpty(requestId))
                return requestId;
            return string.Empty;
        }
    }
}
