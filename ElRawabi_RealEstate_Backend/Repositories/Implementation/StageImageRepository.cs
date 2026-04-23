using ElRawabi_RealEstate_Backend.Data;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ElRawabi_RealEstate_Backend.Repositories.Implementations
{
    public class StageImageRepository : IStageImageRepository
    {
        private readonly ElRawabiRealEstateDbContext _ctx;

        public StageImageRepository(ElRawabiRealEstateDbContext ctx)
            => _ctx = ctx;

        public async Task<IEnumerable<StageImage>> GetByStageIdAsync(int stageId)
            => await _ctx.StageImages
                .Where(si => si.ConstructionStageId == stageId && !si.IsDeleted)
                .OrderBy(si => si.CreatedAt)
                .ToListAsync();

        public async Task<StageImage?> GetByIdAsync(int id)
            => await _ctx.StageImages
                .FirstOrDefaultAsync(si => si.Id == id && !si.IsDeleted);

        public async Task AddAsync(StageImage image)
            => await _ctx.StageImages.AddAsync(image);

        public void Delete(StageImage image)
        {
            image.IsDeleted = true;
            _ctx.StageImages.Update(image);
        }
    }
}