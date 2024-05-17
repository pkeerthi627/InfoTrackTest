using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoMapper;
using Infotrack.Api.Settlement.Dtos;
using Infotrack.Api.Settlement.Infrastructure;
using Infotrack.Api.Settlement.Infrastructure.Data;
using Infotrack.Api.Settlement.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infotrack.Api.UnitTests.Services
{
    public class BookingSettlementServiceTests
    {
        private readonly IBookingSettlementService _bookingSettlementService;
        private readonly ApiDbContext _dbContext;
        private readonly ILogger<BookingSettlementService> _logger;
        private readonly IMapper _mapper;
        private readonly IFixture _fixture;

        public BookingSettlementServiceTests()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApiDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            _dbContext = new ApiDbContext(optionsBuilder.Options);
            var mapperProfile = new SettlementMapperProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(mapperProfile));
            _mapper = new Mapper(config);
            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
            _logger = _fixture.Freeze<ILogger<BookingSettlementService>>();
            _bookingSettlementService = new BookingSettlementService(_logger, _dbContext, _mapper);
        }

        [Fact]
        public async Task BookSettlement_SuccessfulBooking()
        {
            //Arrange
            SettlementBookingRequest testRequest = new()
            {
                BookingTime = "15:00",
                Name = "TestCase1 Name"
            };
            SettlementBooking dbEntity = new()
            {
                BookingStartTime = TimeSpan.FromHours(10),
                BookingEndTime = TimeSpan.FromHours(10).Add(TimeSpan.FromMinutes(59)),
                BookingId = Guid.NewGuid().ToString(),
                Name = "TestCaseDb Name"
            };
            _dbContext.Bookings.Add(dbEntity);
            _dbContext.SaveChanges();
            

            //Act
            var response = await _bookingSettlementService.BookSettlementAsync(testRequest);

            //Assert
            Assert.NotNull(response);
            Assert.True(!string.IsNullOrEmpty(response.BookingId));
            Assert.True(_dbContext.Bookings.Count() == 2);
        }

        [Fact]
        public async Task BookSettlement_SuccessfulBookingWithNoConcurrentBooking()
        {
            //Arrange
            SettlementBookingRequest testRequest = new()
            {
                BookingTime = "15:00",
                Name = "TestCase1 Name"
            };

            //Act
            var response = await _bookingSettlementService.BookSettlementAsync(testRequest);

            //Assert
            Assert.NotNull(response);
            Assert.True(!string.IsNullOrEmpty(response.BookingId));
            Assert.True(_dbContext.Bookings.Count() == 1);
        }

        [Fact]
        public async Task BookSettlement_Conflict_ConcurrentBookingsPresent()
        {
            //Arrange
            SettlementBookingRequest testRequest = new()
            {
                BookingTime = "15:00",
                Name = "TestCase1 Name"
            };
            SettlementBooking dbEntity = new()
            {
                BookingStartTime = TimeSpan.FromHours(15),
                BookingEndTime = TimeSpan.FromHours(15).Add(TimeSpan.FromMinutes(59)),
                BookingId = Guid.NewGuid().ToString(),
                Name = "TestCaseDb Name"
            };
            _dbContext.Bookings.Add(dbEntity);
            _dbContext.SaveChanges();
            dbEntity.BookingId = Guid.NewGuid().ToString();
            _dbContext.Bookings.Add(dbEntity);
            _dbContext.SaveChanges();
            dbEntity.BookingId = Guid.NewGuid().ToString();
            _dbContext.Bookings.Add(dbEntity);
            _dbContext.SaveChanges();
            dbEntity.BookingId = Guid.NewGuid().ToString();
            _dbContext.Bookings.Add(dbEntity);
            _dbContext.SaveChanges();

            //Act
            var response = await _bookingSettlementService.BookSettlementAsync(testRequest);

            //Assert
            Assert.NotNull(response);
            Assert.True(string.IsNullOrEmpty(response.BookingId));
            Assert.True(_dbContext.Bookings.Count() == 4);
        }

    }
}
