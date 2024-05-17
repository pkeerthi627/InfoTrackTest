using FluentValidation.Results;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Infotrack.Api.Settlement.ApiProblemDetails
{
    [ExcludeFromCodeCoverage(Justification = "This is API Problem Details hence excluding from code coverage")]
    public class BadRequestProblem: HttpValidationProblemDetails
    {
        public BadRequestProblem(string? type, string? title, IDictionary<string, List<ValidationFailure>> details, string? requestId = "") 
        {
            Type = type;
            Title = title;
            Extensions.Add("details", details);
            Extensions.Add("requestId", requestId);
            Status = (int)HttpStatusCode.BadRequest;
        }
    }
}
