using AutoMapper;
using Infotrack.Api.Settlement.Dtos;
using Infotrack.Api.Settlement.Infrastructure;
using Infotrack.Api.Settlement.Infrastructure.Data;

namespace Infotrack.Api.UnitTests.Mappers
{
    public class BookingSettlementMapperTests
    {
        private readonly Mapper _mapper;

        public BookingSettlementMapperTests()
        {
            var mapperProfile = new SettlementMapperProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(mapperProfile));
            _mapper = new Mapper(config);
        }

        [Fact]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task AutoMap_AssertConfiguration_IsValid()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            //Arrange
            //Act
            //Assert
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Fact]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task AutoMap_AssertMappingModelToDb_Success()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            //Arrange
            SettlementBooking settlementBooking = new()
            {
                BookingStartTime = TimeSpan.FromHours(10),
                BookingEndTime = TimeSpan.FromHours(10).Add(TimeSpan.FromMinutes(59)),
                BookingId = Guid.NewGuid().ToString(),
                Name = "TestCase1"
            };

            SettlementBookingRequest settlementBookingRequest = new()
            {
                BookingTime = "10:00",
                Name = "TestCase1"
            };
            //Act
            var response = _mapper.Map<SettlementBooking>(settlementBookingRequest);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(response.Name, settlementBooking.Name);
        }


        [Fact]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task AutoMap_AssertMappingDbToResponse_Success()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            //Arrange
            string guid = Guid.NewGuid().ToString();
            SettlementBooking settlementBooking = new()
            {
                BookingStartTime = TimeSpan.FromHours(10),
                BookingEndTime = TimeSpan.FromHours(10).Add(TimeSpan.FromMinutes(59)),
                BookingId = guid,
                Name = "TestCase1"
            };

            //Act
            var response = _mapper.Map<SettlementBookingResponse>(settlementBooking);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(response.BookingId, settlementBooking.BookingId);
        }

    }
}
