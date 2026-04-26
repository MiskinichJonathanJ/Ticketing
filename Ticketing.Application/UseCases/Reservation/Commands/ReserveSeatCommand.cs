using MediatR;
using Ticketing.Application.DTOs;

namespace Ticketing.Application.UseCases.Reservation.Commands
{
    public class ReserveSeatCommand(Guid seatId, int userId) : IRequest<ReservationDto>
    {
        public Guid SeatId { get; set; } = seatId;
        public int UserId { get; set; } = userId;
    }
}
