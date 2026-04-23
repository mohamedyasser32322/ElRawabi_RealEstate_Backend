using ElRawabi_RealEstate_Backend.Data;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<ConstructionStage>> GetAllConstructionStagesAsync()
            => await _dbSet
                .Where(cs => !cs.IsDeleted)
                .Include(cs => cs.Building)
                .ToListAsync();

        public async Task<ConstructionStage?> GetConstructionStageByIdAsync(int id)
            => await _dbSet
                .Include(cs => cs.Building)
                .FirstOrDefaultAsync(cs => cs.Id == id && !cs.IsDeleted);

        public async Task AddConstructionStageAsync(ConstructionStage constructionStage)
            => await _dbSet.AddAsync(constructionStage);

        public void UpdateConstructionStage(ConstructionStage constructionStage)
        {
            constructionStage.UpdatedAt = DateTime.UtcNow;
            _dbSet.Update(constructionStage);
        }

        public void DeleteConstructionStage(ConstructionStage constructionStage)
        {
            constructionStage.IsDeleted = true;
            constructionStage.UpdatedAt = DateTime.UtcNow;
            _dbSet.Update(constructionStage);
        }

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public async Task<IEnumerable<ConstructionStage>> GetConstructionStagesByBuildingIdAsync(int buildingId)
            => await _dbSet
                .Where(cs => cs.BuildingId == buildingId && !cs.IsDeleted)
                .Include(cs => cs.Building)
                .OrderBy(cs => cs.CreatedAt)
                .ToListAsync();
    }
}