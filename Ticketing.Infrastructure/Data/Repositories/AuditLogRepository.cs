using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Interfaces;
using Ticketing.Domain.Entities;

namespace Ticketing.Infrastructure.Data.Repositories
{
    public class AuditLogRepository(DbContext context) : IAuditLogRepository
    {
        private readonly DbContext _context = context;
        public Task AddAuditLogAsync(AuditLog log)
        {
            throw new NotImplementedException();
        }
    }
}
