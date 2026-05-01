using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Interfaces;
using Ticketing.Domain.Entities;

namespace Ticketing.Infrastructure.Data.Repositories
{
    public class SectorRepository(TicketingDbContext context) : ISectorRepository
    {
        private readonly TicketingDbContext _context = context;

        public async Task<IEnumerable<Sector>> GetByEventIdAsync(int eventId)
        {
            return await _context.SECTOR
                .Where(s => s.EventId == eventId)
                .ToListAsync();
        }
    }
}