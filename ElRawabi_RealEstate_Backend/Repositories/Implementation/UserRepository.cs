using ElRawabi_RealEstate_Backend.Data;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElRawabi_RealEstate_Backend.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ElRawabiRealEstateDbContext _context;
        private readonly DbSet<User> _dbSet;

        public UserRepository(ElRawabiRealEstateDbContext context)
        {
            _context = context;
            _dbSet = context.Set<User>();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync() => await _dbSet.ToListAsync();
        public async Task<User?> GetUserByIdAsync(int id) => await _dbSet.FindAsync(id);
        public async Task AddUserAsync(User user) => await _dbSet.AddAsync(user);
        public void UpdateUser(User user) => _dbSet.Update(user);
        public void DeleteUser(User user) => _dbSet.Remove(user);
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}