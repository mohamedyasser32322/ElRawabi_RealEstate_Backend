using ElRawabi_RealEstate_Backend.Data;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<Unit>> GetAllUnitsAsync()
        {
            return await _dbSet
                .Where(u => !u.IsDeleted)
                .Include(u => u.Floor)
                .Include(u => u.Booking)
                .ToListAsync();
        }

        public async Task<Unit?> GetUnitByIdAsync(int id)
        {
            return await _dbSet
                .Include(u => u.Floor)
                .Include(u => u.Booking)
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
        }

        // ✅ ميثود جديدة — جلب وحدات العميل
        public async Task<IEnumerable<Unit>> GetUnitsByBuyerIdAsync(int buyerId)
        {
            return await _dbSet
                .Where(u => u.BuyerId == buyerId && !u.IsDeleted)
                .Include(u => u.Booking)
                .ToListAsync();
        }

        public async Task AddUnitAsync(Unit unit) => await _dbSet.AddAsync(unit);
        public void UpdateUnit(Unit unit) => _dbSet.Update(unit);
        public void DeleteUnit(Unit unit) => _dbSet.Remove(unit);
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<IEnumerable<Unit>> GetUnitsByFloorIdAsync(int floorId)
        {
            return await _dbSet
                .Where(u => u.FloorId == floorId && !u.IsDeleted)
                .Include(u => u.Floor)
                .Include(u => u.Booking)
                .ToListAsync();
        }

        public async Task<IEnumerable<Unit>> GetUnitsByStatusAsync(UnitStatus status)
        {
            return await _dbSet
                .Where(u => u.Status == status)
                .Include(u => u.Floor)
                .Include(u => u.Booking)
                .ToListAsync();
        }
    }
}