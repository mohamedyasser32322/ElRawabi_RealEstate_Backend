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
        private readonly IActivityLogService _activityLogService;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper, IActivityLogService activityLogService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _activityLogService = activityLogService;
        }

        public async Task<IEnumerable<BookingResponseDto>> GetAllBookingsAsync() => _mapper.Map<IEnumerable<BookingResponseDto>>(await _unitOfWork.Bookings.GetAllBookingsAsync());
        public async Task<BookingResponseDto?> GetBookingByIdAsync(int id) { var b = await _unitOfWork.Bookings.GetBookingByIdAsync(id); return b == null ? null : _mapper.Map<BookingResponseDto>(b); }

        public async Task<BookingResponseDto> CreateBookingAsync(BookingRequestDto bookingDto, int? currentUserId)
        {
            var booking = _mapper.Map<Booking>(bookingDto);
            await _unitOfWork.Bookings.AddBookingAsync(booking);
            var unit = await _unitOfWork.Units.GetUnitByIdAsync(bookingDto.UnitId);
            if (unit != null)
            {
                unit.Status = UnitStatus.Reserved;
                unit.BuyerId = bookingDto.BuyerId;
                var floor = await _unitOfWork.Floors.GetFloorByIdAsync(unit.FloorId);
                var building = await _unitOfWork.Buildings.GetBuildingByIdAsync(floor.BuildingId);
                building.AvailableUnits--;
                var project = await _unitOfWork.Projects.GetProjectByIdAsync(building.ProjectId);
                project.AvailableUnits--;
            }
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("حجز", "وحدة", booking.UnitId, $"تم حجز الوحدة للعميل {booking.BuyerId}", currentUserId);
            return _mapper.Map<BookingResponseDto>(booking);
        }

        public async Task<bool> UpdateBookingAsync(int id, BookingRequestDto bookingDto, int? currentUserId)
        {
            var booking = await _unitOfWork.Bookings.GetBookingByIdAsync(id);
            if (booking == null) return false;
            _mapper.Map(bookingDto, booking);
            _unitOfWork.Bookings.UpdateBooking(booking);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("تعديل", "حجز", id, $"تم تعديل بيانات الحجز رقم {id}", currentUserId);
            return true;
        }

        public async Task<bool> DeleteBookingAsync(int id, int? currentUserId)
        {
            var booking = await _unitOfWork.Bookings.GetBookingByIdAsync(id);
            if (booking == null) return false;
            booking.IsDeleted = true;
            var unit = await _unitOfWork.Units.GetUnitByIdAsync(booking.UnitId);
            if (unit != null)
            {
                unit.Status = UnitStatus.Available;
                unit.BuyerId = null;
                var floor = await _unitOfWork.Floors.GetFloorByIdAsync(unit.FloorId);
                var building = await _unitOfWork.Buildings.GetBuildingByIdAsync(floor.BuildingId);
                building.AvailableUnits++;
                var project = await _unitOfWork.Projects.GetProjectByIdAsync(building.ProjectId);
                project.AvailableUnits++;
            }
            _unitOfWork.Bookings.UpdateBooking(booking);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("حذف", "حجز", id, $"تم حذف الحجز رقم {id}", currentUserId);
            return true;
        }
    }
}