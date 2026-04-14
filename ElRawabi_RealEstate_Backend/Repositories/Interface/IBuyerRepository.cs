using ElRawabi_RealEstate_Backend.Modals;

namespace ElRawabi_RealEstate_Backend.Repositories.Interface
{
    public interface IBuyerRepository
    {
        Task<IEnumerable<Buyer>> GetAllBuyersAsync();
        Task<Buyer?> GetBuyerByIdAsync(int id);
        Task AddBuyerAsync(Buyer buyer);
        void UpdateBuyer(Buyer buyer);
        void DeleteBuyer(Buyer buyer);
        Task SaveChangesAsync();
        Task<Buyer?> GetBuyerByNationalIdAsync(string nationalId);
        Task<Buyer?> GetBuyerByEmailAsync(string email);
    }
}
