using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.DTOs.Responses;

namespace ElRawabi_RealEstate_Backend.Services.Interface
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingResponseDto>> GetAllBookingsAsync();
        Task<BookingResponseDto?> GetBookingByIdAsync(int id);
        Task<BookingResponseDto> CreateBookingAsync(BookingRequestDto bookingDto);
        Task<bool> UpdateBookingAsync(int id, BookingRequestDto bookingDto);
        Task<bool> DeleteBookingAsync(int id);
    }
}
