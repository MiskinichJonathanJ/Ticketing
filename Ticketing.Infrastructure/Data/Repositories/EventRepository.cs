using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Interfaces;
using Ticketing.Domain.Entities;

namespace Ticketing.Infrastructure.Data.Repositories
{
    public class EventRepository(TicketingDbContext context) : IEventRepository
    {
        private readonly TicketingDbContext _context = context;

        public async Task AddAsync(Event ev)
        {
            await _context.EVENT.AddAsync(ev);
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _context.EVENT.ToListAsync(); ;
        }

        public async Task<Event?> GetByIdWithSectorsAsync(int id)
        {
            return await _context.EVENT
            .Include(e => e.Sectors)
            .FirstOrDefaultAsync(e => e.Id == id);
        }


    }
}
