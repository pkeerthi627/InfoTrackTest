using AutoMapper;
using Infotrack.Api.Settlement.Dtos;
using Infotrack.Api.Settlement.Infrastructure.Data;
using System.Globalization;

namespace Infotrack.Api.Settlement.Services
{
    public interface IBookingSettlementService
    {
        Task<SettlementBookingResponse> BookSettlementAsync(SettlementBookingRequest request);
    }
    public class BookingSettlementService : IBookingSettlementService
    {
        private readonly ILogger<BookingSettlementService> _logger;
        private readonly ApiDbContext _dbContext;
        private readonly IMapper _mapper;
        public BookingSettlementService(ILogger<BookingSettlementService> logger,
            ApiDbContext dbContext,
            IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<SettlementBookingResponse> BookSettlementAsync(SettlementBookingRequest request)
        {
            _logger.BeginScope("{Operation}", nameof(BookSettlementAsync));
            var searchBookings = _dbContext.Bookings.Where(x => x.BookingStartTime <= TimeSpan.Parse(request.BookingTime!, new CultureInfo("en-AU"))
            && x.BookingEndTime >= TimeSpan.Parse(request.BookingTime!, new CultureInfo("en-AU"))).Count();
            if (searchBookings >= 4)
            {
                return new SettlementBookingResponse();
            }
            var dbRequest = _mapper.Map<SettlementBooking>(request);
            var response = await _dbContext.AddAsync<SettlementBooking>(dbRequest);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<SettlementBookingResponse>(response.Entity);
        }
    }
}
