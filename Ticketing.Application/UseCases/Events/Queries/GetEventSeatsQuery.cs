using MediatR;
using Ticketing.Application.DTOs;

namespace Ticketing.Application.UseCases.Events.Queries
{
    public class GetEventSeatsQuery : IRequest<EventDto> //Cambiar por otro el eventDto por  otro  con mas  sentido, pero por ahora lo dejo asi
    {
    }
}
