using AutoMapper;

namespace Ticketing.Application.Mappings
{
    public class ReservationProfile : Profile
    {
        public ReservationProfile()
        {
            CreateMap<Domain.Entities.Reservation, DTOs.ReservationDto>();
        }
    }
}
