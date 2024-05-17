using Infotrack.Api.Settlement.ApiProblemDetails;
using Infotrack.Api.Settlement.Dtos;
using Infotrack.Api.Settlement.Infrastructure;
using Infotrack.Api.Settlement.Services;
using System.Net;

namespace Infotrack.Api.Settlement.Handlers.Settlement
{
    public class SettlementBookingHandler : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            MapBooking(app);
            
            static void MapBooking(IEndpointRouteBuilder app)
            {
                static async Task<IResult> PostSettlementBookingAsync(SettlementBookingRequest settlementBookingRequest, IBookingSettlementService bookingSettlementService)
                {
                    var response = await bookingSettlementService.BookSettlementAsync(settlementBookingRequest);

                    if (!string.IsNullOrEmpty(response.BookingId))
                    {
                        return TypedResults.Ok(new ApiResponse<SettlementBookingResponse>()
                        {
                            Data = response,
                            Status = "Success",
                            Message = "Booking made Successfully."
                        });
                    }
                    return TypedResults.Conflict(new ApiResponse<object>()
                    {
                        Status = "Failed",
                        Message = "Booking Can not be made because of Conflict"
                    });
                }

                app.MapPost("api/Settlement/Booking", PostSettlementBookingAsync)
                    .WithName("SettlementBooking")
                    .Produces<BadRequestProblem>((int)HttpStatusCode.BadRequest)
                    .Produces<NotFoundProblemDetails>((int)HttpStatusCode.NotFound)
                    .Produces<ApiProblem>((int)HttpStatusCode.InternalServerError)
                    .Produces<ApiResponse<SettlementBookingResponse>>((int)HttpStatusCode.OK)
                    .AddEndpointFilter<SettlementBookingValidationFilter<SettlementBookingRequest>>()
                    .WithOpenApi();
            }
        }
    }
}
