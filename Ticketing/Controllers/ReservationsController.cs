using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Application.UseCases.Reservation.Commands.ReserveSeat;

namespace Ticketing.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReservationsController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Realiza el intento inicial de reserva de un asiento.
        /// </summary>
        /// <remarks>
        /// Esta operación realiza tres acciones atómicas: 
        /// 1. Cambia el estado de la butaca a 'Reserved'[cite: 1].
        /// 2. Crea el registro de reserva temporal con expiración de 5 minutos[cite: 1].
        /// 3. Genera un log de auditoría inmutable detallando el milisegundo exacto de la acción[cite: 1].
        /// </remarks>
        /// <param name="request">Datos del intento de reserva (SeatId y UserId)[cite: 1].</param>
        /// <returns>Los detalles de la reserva creada, incluyendo la fecha de expiración[cite: 1].</returns>
        /// <response code="201">Reserva creada exitosamente; el asiento queda bloqueado por 5 minutos[cite: 1].</response>
        /// <response code="400">Si la solicitud es inválida o el asiento ya no está disponible[cite: 1].</response>
        /// <response code="404">Si el asiento o el usuario no existen[cite: 1].</response>
        /// <response code="409">Conflicto: El asiento fue reservado por otro usuario en el mismo instante[cite: 1].</response>
        [HttpPost]
        [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetReservationById(Guid id)
        {
            return NotFound();
        }

        /// <summary>
        /// Realiza el intento inicial de reserva de un asiento.
        /// </summary>
        /// <remarks>
        /// Esta operación realiza tres acciones atómicas: 
        /// 1. Cambia el estado de la butaca a 'Reserved'[cite: 1].
        /// 2. Crea el registro de reserva temporal con expiración de 5 minutos[cite: 1].
        /// 3. Genera un log de auditoría inmutable detallando el milisegundo exacto de la acción[cite: 1].    
        /// </remarks>
        /// <param name="request">Datos del intento de reserva (SeatId y UserId)[cite: 1].</param>
        /// <returns>Los detalles de la reserva creada, incluyendo la fecha de expiración[cite: 1].</returns>  
        /// <response code="201">Reserva creada exitosamente; el asiento queda bloqueado por 5 minutos[cite: 1].</response>
        /// <response code="400">Si la solicitud es inválida o el asiento ya no está disponible[cite: 1].</response>
        /// <response code="404">Si el asiento o el usuario no existen[cite: 1].</response>
        /// <response code="409">Conflicto: El asiento fue reservado por otro usuario en el mismo instante[cite: 1].</response>
        [HttpPost]
        [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateReservation([FromBody] ReserveSeatCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetReservationById), new { id = result.Id }, result);
        }
    }
}
