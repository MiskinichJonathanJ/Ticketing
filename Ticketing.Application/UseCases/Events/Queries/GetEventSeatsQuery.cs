using MediatR;
using Ticketing.Application.DTOs;

namespace Ticketing.Application.UseCases.Events.Queries
{
    public class GetEventSeatsQuery : IRequest<List<SeatDto>>
    {
    }
}
