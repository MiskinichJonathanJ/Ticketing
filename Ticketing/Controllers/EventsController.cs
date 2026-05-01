using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Application.UseCases.Events.Queries.GetAllEvents;
using Ticketing.Application.UseCases.Seats.Queries.GetSeatsByEvent;
using Ticketing.Application.UseCases.Sectors.Queries.GetSectorsByEvent;



namespace Ticketing.Controllers
{
    [Route("api/v1/events")]
    [ApiController]
    public class EventsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Obtiene la lista completa de eventos disponibles.
        /// </summary>
        /// <remarks>
        /// Este endpoint es utilizado por la interfaz principal para mostrar el catálogo de eventos.
        /// </remarks>
        /// <returns>Una lista de objetos Event que representan los eventos programados.</returns>
        /// <response code="200">Retorna la lista de eventos exitosamente.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EventDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _mediator.Send(new GetEventsQuery());
            return Ok(events);
        }


        /// <summary>
        /// Obtiene el estado actual de todos los asientos de un sector específico.
        /// </summary>
        /// <remarks>
        /// Permite al frontend diferenciar visualmente las butacas disponibles de las ocupadas[cite: 1].
        /// </remarks>
        /// <param name="sectorId">Identificador del sector para consultar sus butacas[cite: 1].</param>
        /// <returns>Lista de asientos con su identificador de fila, número y estado actual[cite: 1].</returns>
        /// <response code="200">Retorna el mapa de asientos del sector[cite: 1].</response>
        /// <response code="404">Si el sector no existe en el sistema[cite: 1].</response>
        [HttpGet("sectors/{sectorId}/seats")]
        [ProducesResponseType(typeof(IEnumerable<SeatDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEventSeats(int id)
        {
            var seats = await _mediator.Send(new GetEventSeatsQuery { EventId = id });
            return Ok(seats);
        }

        /// <summary>
        /// Obtiene los sectores asociados a un evento específico.
        /// </summary>
        /// <param name="eventId">Identificador único del evento.</param>
        /// <returns>Lista de sectores con su respectiva capacidad y precio.</returns>
        /// <response code="200">Retorna los sectores del evento solicitado.</response>
        /// <response code="404">Si el evento especificado no existe[cite: 1].</response>
        [HttpGet("events/{eventId}/sectors")]
        [ProducesResponseType(typeof(IEnumerable<SectorDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEventSectors(int id)
        {
            var sectors = await _mediator.Send(new GetSectorsByEventQuery { EventId = id });
            return Ok(sectors);
        }
    }
}
