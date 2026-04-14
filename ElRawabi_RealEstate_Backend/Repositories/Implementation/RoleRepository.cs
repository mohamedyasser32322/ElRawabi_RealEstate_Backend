using ElRawabi_RealEstate_Backend.Data;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElRawabi_RealEstate_Backend.Repositories.Implementations
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ElRawabiRealEstateDbContext _context;
        private readonly DbSet<Role> _dbSet;

        public RoleRepository(ElRawabiRealEstateDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Role>();
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync() => await _dbSet.ToListAsync();
        public async Task<Role?> GetRoleByIdAsync(int id) => await _dbSet.FindAsync(id);
        public async Task AddRoleAsync(Role role) => await _dbSet.AddAsync(role);
        public void UpdateRole(Role role) => _dbSet.Update(role);
        public void DeleteRole(Role role) => _dbSet.Remove(role);
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<Role?> GetRoleByNameAsync(string roleName)
        {
            return await _dbSet.FirstOrDefaultAsync(r => r.RoleName == roleName);
        }
    }
}


