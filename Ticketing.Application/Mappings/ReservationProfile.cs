using AutoMapper;

namespace Ticketing.Application.Mappings
{
    public class ReservationProfile : Profile
    {
        public ReservationProfile()
        {
            CreateMap<Domain.Entities.Reservation, DTOs.ReservationDto>()
            .ForMember(dest => dest.ExpiresAt, opt => opt.MapFrom(src => src.ExpireAt))
            .ForMember(dest => dest.ReservationId, opt => opt.MapFrom(src => src.Id));



        }
    }
}
