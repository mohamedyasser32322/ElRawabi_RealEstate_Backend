using AutoMapper;
using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.DTOs.Responses;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using ElRawabi_RealEstate_Backend.Services.Interface;

namespace ElRawabi_RealEstate_Backend.Services.Implementation
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookingResponseDto>> GetAllBookingsAsync()
        {
            var bookings = await _unitOfWork.Bookings.GetAllBookingsAsync();
            return _mapper.Map<IEnumerable<BookingResponseDto>>(bookings);
        }

        public async Task<BookingResponseDto?> GetBookingByIdAsync(int id)
        {
            var booking = await _unitOfWork.Bookings.GetBookingByIdAsync(id);
            if (booking == null) return null;
            return _mapper.Map<BookingResponseDto>(booking);
        }

        public async Task<BookingResponseDto> CreateBookingAsync(BookingRequestDto bookingDto)
        {
            var booking = _mapper.Map<Booking>(bookingDto);
            await _unitOfWork.Bookings.AddBookingAsync(booking);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<BookingResponseDto>(booking);
        }

        public async Task<bool> UpdateBookingAsync(int id, BookingRequestDto bookingDto)
        {
            var booking = await _unitOfWork.Bookings.GetBookingByIdAsync(id);
            if (booking == null) return false;

            _mapper.Map(bookingDto, booking);
            _unitOfWork.Bookings.UpdateBooking(booking);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteBookingAsync(int id)
        {
            var booking = await _unitOfWork.Bookings.GetBookingByIdAsync(id);
            if (booking == null) return false;

            booking.IsDeleted = true;
            _unitOfWork.Bookings.UpdateBooking(booking);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
