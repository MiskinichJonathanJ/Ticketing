using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Application.UseCases.Reservation.Commands.ReserveSeat;

namespace Ticketing.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReservationController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] ReserveSeatCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
