using ElRawabi_RealEstate_Backend.Data;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ElRawabi_RealEstate_Backend.Repositories.Implementations
{
    public class FloorRepository : IFloorRepository
    {
        private readonly ElRawabiRealEstateDbContext _context;
        private readonly DbSet<Floor> _dbSet;

        public FloorRepository(ElRawabiRealEstateDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Floor>();
        }

        public async Task<IEnumerable<Floor>> GetAllFloorsAsync() => await _dbSet.ToListAsync();
        public async Task<Floor?> GetFloorByIdAsync(int id) => await _dbSet.FindAsync(id);
        public async Task AddFloorAsync(Floor floor) => await _dbSet.AddAsync(floor);
        public void UpdateFloor(Floor floor) => _dbSet.Update(floor);
        public void DeleteFloor(Floor floor) => _dbSet.Remove(floor); // Consider soft delete logic here if applicable
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<IEnumerable<Floor>> GetFloorsByBuildingIdAsync(int buildingId)
        {
            return await _dbSet.Where(f => f.BuildingId == buildingId).ToListAsync();
        }
    }
}