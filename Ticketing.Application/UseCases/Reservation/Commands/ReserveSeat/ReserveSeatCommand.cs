using MediatR;
using Ticketing.Application.DTOs;

namespace Ticketing.Application.UseCases.Reservation.Commands.ReserveSeat
{
    public class ReserveSeatCommand(Guid seatId, int userId) : IRequest<ReservationDto>
    {
        public Guid SeatId { get; init; } = seatId;
        public int UserId { get; init; } = userId;
    }
}
