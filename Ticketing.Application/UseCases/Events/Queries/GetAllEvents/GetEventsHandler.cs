using AutoMapper;
using MediatR;
using Ticketing.Application.DTOs;
using Ticketing.Application.Interfaces;

namespace Ticketing.Application.UseCases.Events.Queries.GetAllEvents
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
            var events = await _eventRepository.GetAllAsync(); 

            return _mapper.Map<List<EventDto>>(events); 
        }
    }
}
