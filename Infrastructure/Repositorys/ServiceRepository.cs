using Domain.Entities;
using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositorys
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly ApplicationDbContext _context;

        public ServiceRepository(ApplicationDbContext context) => _context = context;

        public async Task AddAsync(Service service)
        { 
           _context.Add(service);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Service service)
        {
            _context.Update(service);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Service>> GetAllAsync()  => await _context.Services.Where(x=>x.Activo == true).ToListAsync();

        public async Task<Service?> GetByIdAsync(int id) => await _context.Services.FindAsync(id);

        public async Task UpdateAsync(Service service)
        {
            _context.Update(service);
            await _context.SaveChangesAsync();
        }
    }
}
