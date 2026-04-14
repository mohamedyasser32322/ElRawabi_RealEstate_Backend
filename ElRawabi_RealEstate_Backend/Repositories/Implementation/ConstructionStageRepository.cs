using ElRawabi_RealEstate_Backend.Data;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ElRawabi_RealEstate_Backend.Repositories.Implementations
{
    public class ConstructionStageRepository : IConstructionStageRepository
    {
        private readonly ElRawabiRealEstateDbContext _context;
        private readonly DbSet<ConstructionStage> _dbSet;

        public ConstructionStageRepository(ElRawabiRealEstateDbContext context)
        {
            _context = context;
            _dbSet = context.Set<ConstructionStage>();
        }

        public async Task<IEnumerable<ConstructionStage>> GetAllConstructionStagesAsync() => await _dbSet.ToListAsync();
        public async Task<ConstructionStage?> GetConstructionStageByIdAsync(int id) => await _dbSet.FindAsync(id);
        public async Task AddConstructionStageAsync(ConstructionStage constructionStage) => await _dbSet.AddAsync(constructionStage);
        public void UpdateConstructionStage(ConstructionStage constructionStage) => _dbSet.Update(constructionStage);
        public void DeleteConstructionStage(ConstructionStage constructionStage) => _dbSet.Remove(constructionStage); // Consider soft delete logic here if applicable
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<IEnumerable<ConstructionStage>> GetConstructionStagesByBuildingIdAsync(int buildingId)
        {
            return await _dbSet.Where(cs => cs.BuildingId == buildingId).ToListAsync();
        }
    }
}