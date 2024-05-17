using System.Diagnostics.CodeAnalysis;

namespace Infotrack.Api.Settlement.Infrastructure
{
    [ExcludeFromCodeCoverage(Justification = "This is the API Response, Hence excluding from coverage")]
    public class ApiResponse<T>
    {
        public string? Status { get; set; }

        public T? Data { get; set; }

        public string? Message { get; set; }
    }
}
