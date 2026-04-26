using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Interfaces;
using Ticketing.Domain.Entities;

namespace Ticketing.Infrastructure.Data.Repositories
{
    public class EventRepository(DbContext context) : IEventRepository
    {
        private readonly DbContext _context = context;

        public Task<IEnumerable<Event>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Event?> GetByIdWithSectorsAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
