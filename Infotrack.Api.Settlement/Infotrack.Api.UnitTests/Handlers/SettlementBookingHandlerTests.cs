using Infotrack.Api.Settlement.Dtos;
using Infotrack.Api.Settlement.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace Infotrack.Api.UnitTests.Handlers
{
    internal class SettlementBookingHandlerTestsApp : WebApplicationFactory<Program>
    {
        private readonly Action<IServiceCollection> _serviceCollection;

        public SettlementBookingHandlerTestsApp(Action<IServiceCollection> serviceCollection)
        {
            _serviceCollection = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(_serviceCollection);
            return base.CreateHost(builder);
        }
    }

    public class SettlementBookingHandlerTests
    {
        private readonly IBookingSettlementService _bookingSettlementService;
        public SettlementBookingHandlerTests()
        {
            _bookingSettlementService = Substitute.For<IBookingSettlementService>();
        }

        [Fact]
        public async Task SettlementBookingHandler_SuccessfulBooking()
        {
            //Arrange
            string orderId = Guid.NewGuid().ToString();
            SettlementBookingRequest settlementBookingRequest = new()
            {
                BookingTime = "15:00",
                Name = "TestCase1"
            };
            SettlementBookingResponse settlementBooking = new()
            {
                BookingId = orderId
            };
            _bookingSettlementService.BookSettlementAsync(Arg.Any<SettlementBookingRequest>()).Returns(settlementBooking);
            using var app = new SettlementBookingHandlerTestsApp(x =>
            {
                x.AddSingleton(_bookingSettlementService);
            });
            var httpClient = app.CreateClient();
            using StringContent jsonContent = new(JsonSerializer.Serialize(settlementBookingRequest), Encoding.UTF8, MediaTypeNames.Application.Json);

            //Act
            var response = await httpClient.PostAsync("/api/Settlement/Booking", jsonContent);

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SettlementBookingHandler_ConflictBooking()
        {
            //Arrange
            SettlementBookingRequest settlementBookingRequest = new()
            {
                BookingTime = "15:00",
                Name = "TestCase1"
            };
            SettlementBookingResponse settlementBooking = new();
            _bookingSettlementService.BookSettlementAsync(Arg.Any<SettlementBookingRequest>()).Returns(settlementBooking);
            using var app = new SettlementBookingHandlerTestsApp(x =>
            {
                x.AddSingleton(_bookingSettlementService);
            });
            var httpClient = app.CreateClient();
            using StringContent jsonContent = new(JsonSerializer.Serialize(settlementBookingRequest), Encoding.UTF8, MediaTypeNames.Application.Json);

            //Act
            var response = await httpClient.PostAsync("/api/Settlement/Booking", jsonContent);

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.Conflict, response.StatusCode);
        }
    }
}
