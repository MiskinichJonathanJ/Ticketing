using Ticketing.Domain.Entities;

namespace Ticketing.Application.Interfaces
{
    public interface ISeatRepository
    {
        Task<IEnumerable<Seat>> GetBySectorIdAsync(int sectorId);
        Task<Seat?> GetByIdAsync(Guid id);
        void Update(Seat seat);
        Task<IEnumerable<Seat>> GetByEventIdAsync(int eventId);
    }
}
