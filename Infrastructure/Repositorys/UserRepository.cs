using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositorys
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<User>> GetAllAsync() => await _context.Users.ToListAsync();

        public async Task<User?> GetByIdAsync(int id) => await _context.Users.FindAsync(id);

        public async Task AddAsync(User product) { _context.Users.Add(product); await _context.SaveChangesAsync(); }

        public async Task DeleteAsync(User product) { _context.Users.Update(product); await _context.SaveChangesAsync(); }

        public async Task UpdateAsync(User product) { _context.Users.Update(product); await _context.SaveChangesAsync(); }
    }
}
