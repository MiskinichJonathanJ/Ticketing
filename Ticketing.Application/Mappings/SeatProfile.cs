using AutoMapper;

namespace Ticketing.Application.Mappings
{
    public class SeatProfile : Profile
    {
        public SeatProfile()
        {
            CreateMap<Domain.Entities.Seat, DTOs.SeatDto>();
        }
    }
}
