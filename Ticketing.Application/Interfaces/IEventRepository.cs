using Ticketing.Domain.Entities;

namespace Ticketing.Application.Interfaces
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllAsync();
        Task<Event?> GetByIdWithSectorsAsync(int id);
    }
}
