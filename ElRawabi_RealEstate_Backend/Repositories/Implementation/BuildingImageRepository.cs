using ElRawabi_RealEstate_Backend.Data;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElRawabi_RealEstate_Backend.Repositories.Implementations
{
    public class BuildingImageRepository : IBuildingImageRepository
    {
        private readonly ElRawabiRealEstateDbContext _context;
        private readonly DbSet<BuildingImage> _dbSet;

        public BuildingImageRepository(ElRawabiRealEstateDbContext context)
        {
            _context = context;
            _dbSet = context.Set<BuildingImage>();
        }

        public async Task<IEnumerable<BuildingImage>> GetAllBuildingImagesAsync() => await _dbSet.ToListAsync();
        public async Task<BuildingImage?> GetBuildingImageByIdAsync(int id) => await _dbSet.FindAsync(id);
        public async Task AddBuildingImageAsync(BuildingImage buildingImage) => await _dbSet.AddAsync(buildingImage);
        public void UpdateBuildingImage(BuildingImage buildingImage) => _dbSet.Update(buildingImage);
        public void DeleteBuildingImage(BuildingImage buildingImage) => _dbSet.Remove(buildingImage);
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<IEnumerable<BuildingImage>> GetBuildingImagesByBuildingIdAsync(int buildingId)
        {
            return await _dbSet.Where(bi => bi.BuildingId == buildingId).ToListAsync();
        }
    }
}