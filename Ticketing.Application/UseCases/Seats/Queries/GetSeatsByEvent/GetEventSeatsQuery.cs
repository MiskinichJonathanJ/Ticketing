using MediatR;
using Ticketing.Application.DTOs;

namespace Ticketing.Application.UseCases.Seats.Queries.GetSeatsByEvent
{
    public class GetEventSeatsQuery : IRequest<List<SeatDto>>
    {
        public int EventId { get; set; }
    }
}
