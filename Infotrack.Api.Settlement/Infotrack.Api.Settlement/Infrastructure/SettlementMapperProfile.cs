using AutoMapper;
using Infotrack.Api.Settlement.Dtos;
using Infotrack.Api.Settlement.Infrastructure.Data;
using System.Globalization;

namespace Infotrack.Api.Settlement.Infrastructure
{
    public class SettlementMapperProfile : Profile
    {
        public SettlementMapperProfile() 
        {
            CreateMap<SettlementBookingRequest, SettlementBooking>()
                .ForMember(x => x.Name, opts => opts.MapFrom(y => y.Name))
                .ForMember(x => x.BookingStartTime, opts => opts.MapFrom(y => TimeSpan.Parse(y.BookingTime!, new CultureInfo("en-AU"))))
                .ForMember(x => x.BookingEndTime, opts => opts.MapFrom(y => TimeSpan.Parse(y.BookingTime!, new CultureInfo("en-AU")).Add(TimeSpan.FromMinutes(59))))
                .ForMember(x => x.BookingId, opts => opts.MapFrom(y => Guid.NewGuid()));

            CreateMap<SettlementBooking, SettlementBookingResponse>()
                .ForMember(x => x.BookingId, opts => opts.MapFrom(y => y.BookingId));
        }
    }
}
