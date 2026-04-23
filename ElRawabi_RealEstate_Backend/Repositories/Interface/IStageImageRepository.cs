using ElRawabi_RealEstate_Backend.Modals;

namespace ElRawabi_RealEstate_Backend.Repositories.Interface
{
    public interface IStageImageRepository
    {
        Task<IEnumerable<StageImage>> GetByStageIdAsync(int stageId);
        Task<StageImage?> GetByIdAsync(int id);
        Task AddAsync(StageImage image);
        void Delete(StageImage image);
    }
}
