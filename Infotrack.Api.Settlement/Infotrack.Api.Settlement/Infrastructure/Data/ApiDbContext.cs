using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Infotrack.Api.Settlement.Infrastructure.Data
{
    [ExcludeFromCodeCoverage(Justification = "This is for In memory Data store - To be included for coverage when moving to actual DB")]
    public class ApiDbContext(DbContextOptions<ApiDbContext> options) : DbContext(options)
    {
        public DbSet<SettlementBooking> Bookings { get; set; }
    }
}
