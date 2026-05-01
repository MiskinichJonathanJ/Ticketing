using Ticketing.Domain.Entities;

namespace Ticketing.Application.Interfaces
{
    public interface IAuditLogRepository
    {
        Task AddAuditLogAsync(AuditLog log);
    }
}
