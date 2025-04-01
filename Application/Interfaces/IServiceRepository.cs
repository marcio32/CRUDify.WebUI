using Domain.Entities;

namespace Application.Interfaces
{
    public interface IServiceRepository
    {
        Task<IEnumerable<Service>> GetAllAsync();
        Task<Service?> GetByIdAsync(int id);
        Task AddAsync(Service product);
        Task UpdateAsync(Service product);
        Task DeleteAsync(Service product);
    }
}
