using ElRawabi_RealEstate_Backend.Modals;

namespace ElRawabi_RealEstate_Backend.Repositories.Interface
{
    public interface IUnitRepository
    {
        Task<IEnumerable<Unit>> GetAllUnitsAsync();
        Task<Unit?> GetUnitByIdAsync(int id);
        Task AddUnitAsync(Unit unit);
        void UpdateUnit(Unit unit);
        void DeleteUnit(Unit unit);
        Task SaveChangesAsync();
        Task<IEnumerable<Unit>> GetUnitsByFloorIdAsync(int floorId);
        Task<IEnumerable<Unit>> GetUnitsByStatusAsync(UnitStatus status);
        Task<IEnumerable<Unit>> GetUnitsByBuyerIdAsync(int buyerId);
    }
}
