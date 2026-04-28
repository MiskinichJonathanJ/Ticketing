using AutoMapper;
using MediatR;
using Ticketing.Application.DTOs;
using Ticketing.Application.Interfaces;

namespace Ticketing.Application.UseCases.Seats.Queries.GetSeatsByEvent
{
    public class GetEventSeatsHandler : IRequestHandler<GetEventSeatsQuery, List<SeatDto>>
    {
        private readonly IEventRepository _eventRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IMapper _mapper;

        public GetEventSeatsHandler(
            IEventRepository eventRepository,
            ISeatRepository seatRepository,
            IMapper mapper)
        {
            _eventRepository = eventRepository;
            _seatRepository = seatRepository;
            _mapper = mapper;
        }

        public async Task<List<SeatDto>> Handle(GetEventSeatsQuery request, CancellationToken cancellationToken)
        {
            
            var eventExists = await _eventRepository.GetByIdWithSectorsAsync(request.EventId);
            if (eventExists is null)
                throw new KeyNotFoundException($"Evento con id {request.EventId} no encontrado.");

            
            var seats = await _seatRepository.GetByEventIdAsync(request.EventId);

            return _mapper.Map<List<SeatDto>>(seats);
        }
    }
}