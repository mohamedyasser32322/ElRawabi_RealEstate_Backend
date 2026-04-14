using ElRawabi_RealEstate_Backend.Modals;

namespace ElRawabi_RealEstate_Backend.Repositories.Interface
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Booking?> GetBookingByIdAsync(int id);
        Task AddBookingAsync(Booking booking);
        void UpdateBooking(Booking booking);
        void DeleteBooking(Booking booking);
        Task SaveChangesAsync();
        Task<IEnumerable<Booking>> GetBookingsByBuyerIdAsync(int buyerId);
        Task<Booking?> GetBookingWithDetailsAsync(int bookingId);
    }
}
