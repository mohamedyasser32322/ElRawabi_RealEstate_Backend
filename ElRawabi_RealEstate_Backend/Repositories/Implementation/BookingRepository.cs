using ElRawabi_RealEstate_Backend.Data;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElRawabi_RealEstate_Backend.Repositories.Implementations
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ElRawabiRealEstateDbContext _context;
        private readonly DbSet<Booking> _dbSet;

        public BookingRepository(ElRawabiRealEstateDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Booking>();
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync() => await _dbSet.ToListAsync();
        public async Task<Booking?> GetBookingByIdAsync(int id) => await _dbSet.FindAsync(id);
        public async Task AddBookingAsync(Booking booking) => await _dbSet.AddAsync(booking);
        public void UpdateBooking(Booking booking) => _dbSet.Update(booking);
        public void DeleteBooking(Booking booking) => _dbSet.Remove(booking); // Consider soft delete logic here if applicable
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<IEnumerable<Booking>> GetBookingsByBuyerIdAsync(int buyerId)
        {
            return await _dbSet.Where(b => b.BuyerId == buyerId).ToListAsync();
        }

        public async Task<Booking?> GetBookingWithDetailsAsync(int bookingId)
        {
            return await _dbSet
                .Include(b => b.Buyer)
                .Include(b => b.Unit)
                .FirstOrDefaultAsync(b => b.Id == bookingId);
        }
    }
}