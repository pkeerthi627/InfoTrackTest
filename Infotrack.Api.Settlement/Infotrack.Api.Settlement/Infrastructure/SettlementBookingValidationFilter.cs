using FluentValidation;
using FluentValidation.Results;
using Infotrack.Api.Settlement.ApiProblemDetails;
using System.Diagnostics.CodeAnalysis;

namespace Infotrack.Api.Settlement.Infrastructure
{
    [ExcludeFromCodeCoverage(Justification = "This is Validation Filter hence skipping Coverage")]
    public class SettlementBookingValidationFilter<T> : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
            if(validator != null)
            {
                var entity = context.Arguments
                    .OfType<T>()
                    .FirstOrDefault(x => x?.GetType() == typeof(T));
                if(!object.Equals(entity, default(T)))
                {
                    var valid = await validator.ValidateAsync(entity!);
                    if(valid.IsValid)
                    {
                        return await next(context);
                    }
                    return Results.BadRequest(
                        new BadRequestProblem("Request Validation Error",
                        "Unsupported Request",
                        new Dictionary<string, List<ValidationFailure>>()
                        {{ "ValidationErrors",valid.Errors }},
                        context.HttpContext.GetRequestIdentifier()
                        ));
                }
                else
                {
                    return Results.Problem("No Type To Validate");
                }
            }
            return await next(context);
        }
    }
}
