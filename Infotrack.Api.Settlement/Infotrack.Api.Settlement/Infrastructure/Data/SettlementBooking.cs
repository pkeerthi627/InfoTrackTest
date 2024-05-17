using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Infotrack.Api.Settlement.Infrastructure.Data
{
    [ExcludeFromCodeCoverage(Justification = "This is Db Entity for In memory Data store - To be included for coverage when moving to actual DB")]
    public class SettlementBooking
    {
        [Key]
        public string? BookingId { get; set; }
        public TimeSpan? BookingStartTime { get; set; }
        public TimeSpan? BookingEndTime { get; set; }
        public string? Name { get; set; }
    }
}
