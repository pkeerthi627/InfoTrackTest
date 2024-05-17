using FluentValidation;
using Infotrack.Api.Settlement.Dtos;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Infotrack.Api.Settlement.Handlers.Settlement
{
    public class SettlementRequestValidator: AbstractValidator<SettlementBookingRequest>
    {
        public SettlementRequestValidator()
        {
            RuleFor(x => x.Name).Must(x => !string.IsNullOrEmpty(x))
                .WithMessage("Name can not be Null Or Empty");
            RuleFor(x => x.BookingTime).Must(x => Regex.IsMatch(x!, @"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$") 
            && TimeOnly.Parse(x!, new CultureInfo("en-AU")) >= TimeOnly.Parse("09:00", new CultureInfo("en-AU"))
            && TimeOnly.Parse(x!, new CultureInfo("en-AU")) <= TimeOnly.Parse("16:00", new CultureInfo("en-AU")))
                .WithMessage("Time requested is not in the right format or outside Business Hours");
        }
    }
}
