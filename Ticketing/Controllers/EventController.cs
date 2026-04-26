using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Application.DTOs;

namespace Ticketing.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EventController : Controller
    {
        private readonly IMediator _mediator;

        public EventController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<EventDto>> GetAllEvents()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public async Task<EventDto> GetEventById(int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}/sectors")]
        public async Task<IEnumerable<SectorDto>> GetEventSectors(int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}/seats")]
        public async Task<IEnumerable<SeatDto>> GetEventSeats(int id)
        {
            throw new NotImplementedException();
        }
    }
}
