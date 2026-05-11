using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Interfaces;
using Ticketing.Domain.Entities;
using Ticketing.Domain.Enums;

namespace Ticketing.Infrastructure.Data.Repositories
{
    public class ReservationRepository(TicketingDbContext context) : IReservationRepository
    {
        private readonly TicketingDbContext _context = context;

        public async Task AddAsync(Reservation reservation)
        {
            await _context.RESERVATION.AddAsync(reservation);
        }

        public async Task<bool> ExistsActiveReservation(Guid seatId)
        {
            return await _context.RESERVATION
            .AnyAsync(r => r.SeatId == seatId
                        && r.Status == ReservationStatus.Pending
                        && r.ExpireAt > DateTime.UtcNow);
        }

        public async Task<Reservation?> GetByIdAsync(Guid reservationId)
        {
            return await _context.RESERVATION
                .Include(r => r.Seat)
                .FirstOrDefaultAsync(r => r.Id == reservationId);
        }

        public void Update(Reservation reservation)
        {
            _context.RESERVATION.Update(reservation);
        }
    }
}
