using AutoMapper;
using MediatR;
using Ticketing.Application.DTOs;
using Ticketing.Application.Interfaces;
using Ticketing.Application.UseCases.Events.Queries;

namespace Ticketing.Application.UseCases.Events.Handlers
{
    public class GetEventsHandler : IRequestHandler<GetEventsQuery, List<EventDto>>
    {

        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;


        public GetEventsHandler (IEventRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }


        public async Task<List<EventDto>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
        {
            var events = await _eventRepository.GetAllAsync(); // trae todos los eventos de la DB

            return _mapper.Map<List<EventDto>>(events); // convierte de entidad a DTO usando AutoMapper
        }
    }
}
