using AutoMapper;

namespace Ticketing.Application.Mappings
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap < Domain.Entities.Event, DTOs.EventDto>();
        }
    }
}
