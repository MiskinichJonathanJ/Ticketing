using MediatR;
using Ticketing.Application.DTOs;
using Ticketing.Application.UseCases.Events.Queries;

namespace Ticketing.Application.UseCases.Events.Handlers
{
    public class GetEventSeatsHandler : IRequestHandler<GetEventSeatsQuery, List<SeatDto>>
    {
        public Task<List<SeatDto>> Handle(GetEventSeatsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
