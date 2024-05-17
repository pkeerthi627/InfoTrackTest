using Infotrack.Api.Settlement.Dtos;
using Infotrack.Api.Settlement.Handlers.Settlement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infotrack.Api.UnitTests.Validators
{
    public class SettlementBookingRequestValidationTests
    {
        SettlementRequestValidator requestValidator = new SettlementRequestValidator();

        [Fact]
        public void Validate_BookingRequest_EmptyName()
        {
            //Arrange
            SettlementBookingRequest settlementBookingRequest = new()
            {
                BookingTime = "15:00",
                Name = string.Empty
            };
            //Act
            var validationResult = requestValidator.Validate(settlementBookingRequest);
            //Assert
            Assert.False(validationResult.IsValid);
            Assert.True(validationResult.Errors.Count > 0);
        }

        [Fact]
        public void Validate_BookingRequest_InvalidTime()
        {
            //Arrange
            SettlementBookingRequest settlementBookingRequest = new()
            {
                BookingTime = "15:0a",
                Name = "Test"
            };
            //Act
            var validationResult = requestValidator.Validate(settlementBookingRequest);
            //Assert
            Assert.False(validationResult.IsValid);
            Assert.True(validationResult.Errors.Count > 0);
        }

        [Fact]
        public void Validate_BookingRequest_ValidData()
        {
            //Arrange
            SettlementBookingRequest settlementBookingRequest = new()
            {
                BookingTime = "15:00",
                Name = "Test"
            };
            //Act
            var validationResult = requestValidator.Validate(settlementBookingRequest);
            //Assert
            Assert.True(validationResult.IsValid);
            Assert.True(validationResult.Errors.Count == 0);
        }

        [Fact]
        public void Validate_BookingRequest_OutOfBusinessHours()
        {
            //Arrange
            SettlementBookingRequest settlementBookingRequest = new()
            {
                BookingTime = "18:00",
                Name = "Test"
            };
            //Act
            var validationResult = requestValidator.Validate(settlementBookingRequest);
            //Assert
            Assert.False(validationResult.IsValid);
            Assert.True(validationResult.Errors.Count > 0);
        }
    }
}
