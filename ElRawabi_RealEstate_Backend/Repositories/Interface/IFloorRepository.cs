using ElRawabi_RealEstate_Backend.Modals;

namespace ElRawabi_RealEstate_Backend.Repositories.Interface
{
    public interface IFloorRepository
    {
        Task<IEnumerable<Floor>> GetAllFloorsAsync();
        Task<Floor?> GetFloorByIdAsync(int id);
        Task AddFloorAsync(Floor floor);
        void UpdateFloor(Floor floor);
        void DeleteFloor(Floor floor);
        Task SaveChangesAsync();
        Task<IEnumerable<Floor>> GetFloorsByBuildingIdAsync(int buildingId);
    }
}
