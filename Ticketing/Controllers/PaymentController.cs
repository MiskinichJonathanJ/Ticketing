using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Application.DTOs;
using Ticketing.Application.UseCases.Payments.Commands.ProcessPayment;

namespace Ticketing.Controllers
{
    [Route("api/v1/payments")]
    [ApiController]
    public class PaymentsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// Procesa el pago de una reserva activa
        /// 1. Cambia el asiento a "Sold"
        /// 2. Marca la reserva como "Paid"
        /// 3. Registra en auditoría
        /// Si algo falla, se ejecuta un Rollback completo.
       
        [HttpPost]
        [ProducesResponseType(typeof(PaymentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}