using MediatR;
using Ticketing.Application.DTOs;

namespace Ticketing.Application.UseCases.Events.Queries
{
    public class GetEventsQuery :  IRequest<List<EventDto>>
    {
    }
}
