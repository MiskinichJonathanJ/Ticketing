using AutoMapper;
using FluentValidation;
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
        private readonly IValidator<GetEventSeatsQuery> _validator;

        public GetEventSeatsHandler(
            IEventRepository eventRepository,
            ISeatRepository seatRepository,
            IMapper mapper,
            IValidator<GetEventSeatsQuery> validator)
        {
            _eventRepository = eventRepository;
            _seatRepository = seatRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<List<SeatDto>> Handle(GetEventSeatsQuery request, CancellationToken cancellationToken)
        {

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var eventExists = await _eventRepository.GetByIdWithSectorsAsync(request.EventId);
            if (eventExists is null)
                throw new KeyNotFoundException($"Evento con id {request.EventId} no encontrado.");

            if (!eventExists.Sectors.Any(s => s.Id == request.SectorId))
                throw new KeyNotFoundException($"El sector {request.SectorId} no pertenece al evento {request.EventId}.");

            var seats = await _seatRepository.GetBySectorIdAsync(request.SectorId);

            return _mapper.Map<List<SeatDto>>(seats);
        }
    }
}