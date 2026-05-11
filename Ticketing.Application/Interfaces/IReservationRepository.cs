using Ticketing.Domain.Entities;

namespace Ticketing.Application.Interfaces
{
    public interface IReservationRepository
    {
        Task AddAsync(Reservation reservation);
        Task<bool> ExistsActiveReservation(Guid seatId);

        Task<Reservation?> GetByIdAsync(Guid reservationId);
        void Update(Reservation reservation);
    }
}
