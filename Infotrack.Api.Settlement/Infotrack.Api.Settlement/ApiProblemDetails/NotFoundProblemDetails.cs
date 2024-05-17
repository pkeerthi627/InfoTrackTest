using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Infotrack.Api.Settlement.ApiProblemDetails
{
    [ExcludeFromCodeCoverage(Justification = "This is API Problem Details hence excluding from code coverage")]
    public class NotFoundProblemDetails : ProblemDetails
    {
        public NotFoundProblemDetails(string? title, string? details = "", string? requestId = "")
        {
            Status = (int)HttpStatusCode.NotFound;
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4";
            Title = title;
            Detail = details;
            Extensions.Add("requestId", requestId);
        }
    }
}
