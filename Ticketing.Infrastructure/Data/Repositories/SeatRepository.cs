using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Interfaces;
using Ticketing.Domain.Entities;

namespace Ticketing.Infrastructure.Data.Repositories
{
    public class SeatRepository(DbContext context) : ISeatRepository
    {
        private readonly DbContext _context = context;
        public Task<Seat?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Seat>> GetBySectorIdAsync(int sectorId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Seat seat)
        {
            throw new NotImplementedException();
        }
    }
}
