using AutoMapper;

namespace Ticketing.Application.Mappings
{
    public class SectorProfile : Profile
    {
        public SectorProfile()
        {
            CreateMap<Domain.Entities.Sector, DTOs.SectorDto>();
        }
    }
}
