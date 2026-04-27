using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Interfaces;
using Ticketing.Domain.Entities;

namespace Ticketing.Infrastructure.Data.Repositories
{
    public class SeatRepository(TicketingDbContext context) : ISeatRepository
    {
        private readonly TicketingDbContext _context = context;
        public async Task<Seat?> GetByIdAsync(Guid id)
        {
            return await _context.SEAT.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Seat>> GetBySectorIdAsync(int sectorId)
        {
            return await _context.SEAT.Where(s => s.SectorId == sectorId).ToListAsync();
        }

        public void Update(Seat seat)
        {
            _context.SEAT.Update(seat);
        }

        public async Task<IEnumerable<Seat>> GetByEventIdAsync(int eventId)
        {
            return await _context.SEAT
                .Include(s => s.Sector)
                .Where(s => s.Sector.EventId == eventId)
                .ToListAsync();
        }
    }
}
