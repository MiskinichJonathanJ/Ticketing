using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Interfaces;
using Ticketing.Domain.Entities;

namespace Ticketing.Infrastructure.Data.Repositories
{
    public class AuditLogRepository(TicketingDbContext context) : IAuditLogRepository
    {
        private readonly TicketingDbContext _context = context;
        public async Task AddAuditLogAsync(AuditLog log)
        {
            await _context.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}
