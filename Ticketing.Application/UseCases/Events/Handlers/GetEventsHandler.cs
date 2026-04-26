using MediatR;
using Ticketing.Application.DTOs;
using Ticketing.Application.UseCases.Events.Queries;

namespace Ticketing.Application.UseCases.Events.Handlers
{
    public class GetEventsHandler : IRequestHandler<GetEventsQuery, List<EventDto>>
    {
        public Task<List<EventDto>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
