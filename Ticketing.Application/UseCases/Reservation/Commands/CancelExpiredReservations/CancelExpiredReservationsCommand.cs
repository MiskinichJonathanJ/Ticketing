using MediatR;

namespace Ticketing.Application.UseCases.Reservation.Commands.CancelExpiredReservations
{
    public record CancelExpiredReservationsCommand : IRequest;
}
