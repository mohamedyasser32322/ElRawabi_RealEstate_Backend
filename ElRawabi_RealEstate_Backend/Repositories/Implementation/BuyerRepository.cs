using ElRawabi_RealEstate_Backend.Data;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElRawabi_RealEstate_Backend.Repositories.Implementations
{
    public class BuyerRepository : IBuyerRepository
    {
        private readonly ElRawabiRealEstateDbContext _context;
        private readonly DbSet<Buyer> _dbSet;

        public BuyerRepository(ElRawabiRealEstateDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Buyer>();
        }

        public async Task<IEnumerable<Buyer>> GetAllBuyersAsync() => await _dbSet.ToListAsync();
        public async Task<Buyer?> GetBuyerByIdAsync(int id) => await _dbSet.FindAsync(id);
        public async Task AddBuyerAsync(Buyer buyer) => await _dbSet.AddAsync(buyer);
        public void UpdateBuyer(Buyer buyer) => _dbSet.Update(buyer);
        public void DeleteBuyer(Buyer buyer) => _dbSet.Remove(buyer);
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<Buyer?> GetBuyerByNationalIdAsync(string nationalId)
        {
            return await _dbSet.FirstOrDefaultAsync(b => b.NationalId == nationalId);
        }

        public async Task<Buyer?> GetBuyerByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(b => b.Email == email);
        }
    }
}