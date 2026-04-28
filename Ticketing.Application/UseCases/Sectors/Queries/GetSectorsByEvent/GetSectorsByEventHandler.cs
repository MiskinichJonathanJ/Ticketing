using AutoMapper;
using MediatR;
using Ticketing.Application.DTOs;
using Ticketing.Application.Interfaces;

namespace Ticketing.Application.UseCases.Sectors.Queries.GetSectorsByEvent
{
    public  class GetSectorsByEventHandler : IRequestHandler<GetSectorsByEventQuery, List<SectorDto>>
    {
        private readonly IEventRepository _eventRepository;
        private readonly ISectorRepository _sectorRepository;
        private readonly IMapper _mapper;

        public GetSectorsByEventHandler(
            IEventRepository eventRepository,
            ISectorRepository sectorRepository,
            IMapper mapper)
        {
            _eventRepository = eventRepository;
            _sectorRepository = sectorRepository;
            _mapper = mapper;
        }

        public async Task<List<SectorDto>> Handle(GetSectorsByEventQuery request, CancellationToken cancellationToken)
        {           
            var eventExists = await _eventRepository.GetByIdWithSectorsAsync(request.EventId);
            if (eventExists is null)
                throw new KeyNotFoundException($"Evento con id {request.EventId} no encontrado.");
        
            
            var sectors = await _sectorRepository.GetByEventIdAsync(request.EventId);

            return _mapper.Map<List<SectorDto>>(sectors);
        }
    }
}
