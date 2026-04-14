using ElRawabi_RealEstate_Backend.Modals;

namespace ElRawabi_RealEstate_Backend.Repositories.Interface
{
    public interface IConstructionStageRepository
    {
        Task<IEnumerable<ConstructionStage>> GetAllConstructionStagesAsync();
        Task<ConstructionStage?> GetConstructionStageByIdAsync(int id);
        Task AddConstructionStageAsync(ConstructionStage constructionStage);
        void UpdateConstructionStage(ConstructionStage constructionStage);
        void DeleteConstructionStage(ConstructionStage constructionStage);
        Task SaveChangesAsync();
        Task<IEnumerable<ConstructionStage>> GetConstructionStagesByBuildingIdAsync(int buildingId);
    }
}
