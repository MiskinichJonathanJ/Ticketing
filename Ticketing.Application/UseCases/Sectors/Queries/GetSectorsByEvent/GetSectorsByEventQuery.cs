using MediatR;
using Ticketing.Application.DTOs;

namespace Ticketing.Application.UseCases.Sectors.Queries.GetSectorsByEvent
{
    public class GetSectorsByEventQuery : IRequest<List<SectorDto>>
    {
        public int EventId { get; set; }

    }
}
