using Ticketing.Domain.Entities;

namespace Ticketing.Application.Interfaces
{
    public interface ISeatRepository
    {
        Task<IEnumerable<Seat>> GetBySectorIdAsync(int sectorId);
        Task<Seat?> GetByIdAsync(Guid id);
        // Devuelve true si la actualización fue exitosa (manejo de concurrencia)
        Task<bool> UpdateAsync(Seat seat);
    }
}
