using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Application.DTOs;
using Ticketing.Application.UseCases.Events.Queries.GetAllEvents;
using Ticketing.Application.UseCases.Seats.Queries.GetSeatsByEvent;
using Ticketing.Application.UseCases.Sectors.Queries.GetSectorsByEvent;



namespace Ticketing.Controllers
{
    [Route("api/v1/events")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET /api/v1/events
        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _mediator.Send(new GetEventsQuery());
            return Ok(events);
        }


        // GET /api/v1/events/{id}/seats
        [HttpGet("{id}/seats")]
        public async Task<IActionResult> GetEventSeats(int id)
        {
            var seats = await _mediator.Send(new GetEventSeatsQuery { EventId = id });
            return Ok(seats);
        }

        // GET /api/v1/events/{id}/sectors
        [HttpGet("{id}/sectors")]
        public async Task<IActionResult> GetEventSectors(int id)
        {
            var sectors = await _mediator.Send(new GetSectorsByEventQuery { EventId = id });
            return Ok(sectors);
        }
    }
}
