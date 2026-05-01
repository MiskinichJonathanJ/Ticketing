using MediatR;
using Ticketing.Application.DTOs;

namespace Ticketing.Application.UseCases.Events.Queries.GetAllEvents
{
    public class GetEventsQuery :  IRequest<List<EventDto>>
    {
    }
}
