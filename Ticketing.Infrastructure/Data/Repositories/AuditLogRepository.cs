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
            await _context.AUDIT_LOG.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task EnqueueAuditLogAsync(AuditLog log)
        {
            await _context.AUDIT_LOG.AddAsync(log);
            // Sin SaveChangesAsync — lo persiste el CommitAsync
        }
    }
}
