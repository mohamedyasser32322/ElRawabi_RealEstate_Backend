using ElRawabi_RealEstate_Backend.Modals;

namespace ElRawabi_RealEstate_Backend.Repositories.Interface
{
    public interface IBuildingRepository
    {
        Task<IEnumerable<Building>> GetAllBuildingsAsync();
        Task<Building?> GetBuildingByIdAsync(int id);
        Task AddBuildingAsync(Building building);
        void UpdateBuilding(Building building);
        void DeleteBuilding(Building building);
        Task SaveChangesAsync();
        Task<IEnumerable<Building>> GetBuildingsByProjectIdAsync(int projectId);
    }
}
