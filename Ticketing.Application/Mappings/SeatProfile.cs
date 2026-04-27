using AutoMapper;

namespace Ticketing.Application.Mappings
{
    public class SeatProfile : Profile
    {
        public SeatProfile()
        {
            CreateMap<Domain.Entities.Seat, DTOs.SeatDto>()
            .ForMember(dest => dest.SectorName, opt => opt.MapFrom(src => src.Sector.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Sector.Price));
        }
    }
}
