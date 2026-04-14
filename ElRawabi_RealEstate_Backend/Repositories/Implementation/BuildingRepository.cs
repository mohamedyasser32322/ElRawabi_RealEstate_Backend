using ElRawabi_RealEstate_Backend.Data;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElRawabi_RealEstate_Backend.Repositories.Implementations
{
    public class BuildingRepository : IBuildingRepository
    {
        private readonly ElRawabiRealEstateDbContext _context;
        private readonly DbSet<Building> _dbSet;

        public BuildingRepository(ElRawabiRealEstateDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Building>();
        }

        public async Task<IEnumerable<Building>> GetAllBuildingsAsync() => await _dbSet.ToListAsync();
        public async Task<Building?> GetBuildingByIdAsync(int id) => await _dbSet.FindAsync(id);
        public async Task AddBuildingAsync(Building building) => await _dbSet.AddAsync(building);
        public void UpdateBuilding(Building building) => _dbSet.Update(building);
        public void DeleteBuilding(Building building) => _dbSet.Remove(building);
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<IEnumerable<Building>> GetBuildingsByProjectIdAsync(int projectId)
        {
            return await _dbSet.Where(b => b.ProjectId == projectId).ToListAsync();
        }
    }
}