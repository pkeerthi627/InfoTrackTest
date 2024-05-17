using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Infotrack.Api.Settlement.ApiProblemDetails
{
    [ExcludeFromCodeCoverage(Justification = "This is API Problem Details hence excluding from code coverage")]
    public class ApiProblem : ProblemDetails
    {
        public ApiProblem(string? type, string? title, string? details, string? requestId = "")
        {
            Status = (int)HttpStatusCode.InternalServerError;
            Title = title;
            Type = type;
            Detail = details;
            Extensions.Add("requestId", requestId);
        }
    }
}
