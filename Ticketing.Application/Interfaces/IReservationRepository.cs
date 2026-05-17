using Ticketing.Domain.Entities;

namespace Ticketing.Application.Interfaces
{
    public interface IReservationRepository
    {
        Task AddAsync(Reservation reservation);
        Task<bool> ExistsActiveReservation(Guid seatId);
        Task<IEnumerable<Reservation>> GetExpiredPendingReservationsAsync(DateTime now);
    }
}
