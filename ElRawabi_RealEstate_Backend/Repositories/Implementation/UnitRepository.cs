using ElRawabi_RealEstate_Backend.Data;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElRawabi_RealEstate_Backend.Repositories.Implementations
{
    public class UnitRepository : IUnitRepository
    {
        private readonly ElRawabiRealEstateDbContext _context;
        private readonly DbSet<Unit> _dbSet;

        public UnitRepository(ElRawabiRealEstateDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Unit>();
        }

        public async Task<IEnumerable<Unit>> GetAllUnitsAsync() => await _dbSet.ToListAsync();
        public async Task<Unit?> GetUnitByIdAsync(int id) => await _dbSet.FindAsync(id);
        public async Task AddUnitAsync(Unit unit) => await _dbSet.AddAsync(unit);
        public void UpdateUnit(Unit unit) => _dbSet.Update(unit);
        public void DeleteUnit(Unit unit) => _dbSet.Remove(unit); // Consider soft delete logic here if applicable
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<IEnumerable<Unit>> GetUnitsByFloorIdAsync(int floorId)
        {
            return await _dbSet.Where(u => u.FloorId == floorId).ToListAsync();
        }

        public async Task<IEnumerable<Unit>> GetUnitsByStatusAsync(UnitStatus status)
        {
            return await _dbSet.Where(u => u.Status == status).ToListAsync();
        }
    }
}