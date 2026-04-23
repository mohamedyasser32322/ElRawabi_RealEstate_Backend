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

        public async Task<IEnumerable<BookingResponseDto>> GetAllBookingsAsync() =>
            _mapper.Map<IEnumerable<BookingResponseDto>>(await _unitOfWork.Bookings.GetAllBookingsAsync());

        public async Task<BookingResponseDto?> GetBookingByIdAsync(int id)
        {
            var b = await _unitOfWork.Bookings.GetBookingByIdAsync(id);
            return b == null ? null : _mapper.Map<BookingResponseDto>(b);
        }

        public async Task<BookingResponseDto> CreateBookingAsync(BookingRequestDto bookingDto, int? currentUserId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var booking = _mapper.Map<Booking>(bookingDto);
                await _unitOfWork.Bookings.AddBookingAsync(booking);
                await _unitOfWork.CompleteAsync();

                var unit = await _unitOfWork.Units.GetUnitByIdAsync(bookingDto.UnitId);
                if (unit != null)
                {
                    unit.Status = UnitStatus.Reserved;
                    unit.BuyerId = bookingDto.BuyerId;
                    unit.BookingId = booking.Id;

                    var floor = await _unitOfWork.Floors.GetFloorByIdAsync(unit.FloorId);
                    if (floor != null)
                    {
                        var building = await _unitOfWork.Buildings.GetBuildingByIdAsync(floor.BuildingId);
                        if (building != null)
                        {
                            building.AvailableUnits--;
                            var project = await _unitOfWork.Projects.GetProjectByIdAsync(building.ProjectId);
                            if (project != null) project.AvailableUnits--;
                        }
                    }
                }

                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();

                var newSnapshot = new
                {
                    UnitId = booking.UnitId,
                    BuyerId = booking.BuyerId,
                    Status = booking.Status.ToString(),
                    BookingDate = booking.BookingDate
                };

                await _activityLogService.LogActivityAsync(
                    "إضافة", "حجز", booking.Id,
                    $"حجز وحدة رقم {booking.UnitId} للعميل {booking.BuyerId}",
                    currentUserId,
                    newValues: newSnapshot);

                return _mapper.Map<BookingResponseDto>(booking);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> UpdateBookingAsync(int id, BookingRequestDto bookingDto, int? currentUserId)
        {
            var booking = await _unitOfWork.Bookings.GetBookingByIdAsync(id);
            if (booking == null) return false;

            var oldSnapshot = new
            {
                UnitId = booking.UnitId,
                BuyerId = booking.BuyerId,
                Status = booking.Status.ToString(),
                BookingDate = booking.BookingDate
            };

            _mapper.Map(bookingDto, booking);

            var unit = await _unitOfWork.Units.GetUnitByIdAsync(booking.UnitId);
            if (unit != null)
            {
                if (unit.BuyerId != booking.BuyerId)
                    unit.BuyerId = booking.BuyerId;

                if (booking.Status == BookingStatus.Cancelled && unit.Status != UnitStatus.Available)
                {
                    unit.Status = UnitStatus.Available;
                    unit.BuyerId = null;
                    unit.BookingId = null;

                    var floor = await _unitOfWork.Floors.GetFloorByIdAsync(unit.FloorId);
                    if (floor != null)
                    {
                        var building = await _unitOfWork.Buildings.GetBuildingByIdAsync(floor.BuildingId);
                        if (building != null)
                        {
                            building.AvailableUnits++;
                            var project = await _unitOfWork.Projects.GetProjectByIdAsync(building.ProjectId);
                            if (project != null) project.AvailableUnits++;
                        }
                    }
                }
                else if (booking.Status == BookingStatus.Confirmed)
                {
                    unit.Status = UnitStatus.Sold;
                }
            }

            _unitOfWork.Bookings.UpdateBooking(booking);
            await _unitOfWork.CompleteAsync();

            var newSnapshot = new
            {
                UnitId = booking.UnitId,
                BuyerId = booking.BuyerId,
                Status = booking.Status.ToString(),
                BookingDate = booking.BookingDate
            };

            await _activityLogService.LogActivityAsync(
                "تعديل", "حجز", id,
                $"تعديل الحجز رقم {id}",
                currentUserId,
                oldValues: oldSnapshot,
                newValues: newSnapshot);

            return true;
        }

        public async Task<bool> DeleteBookingAsync(int id, int? currentUserId)
        {
            var booking = await _unitOfWork.Bookings.GetBookingByIdAsync(id);
            if (booking == null) return false;

            var oldSnapshot = new
            {
                UnitId = booking.UnitId,
                BuyerId = booking.BuyerId,
                Status = booking.Status.ToString(),
                BookingDate = booking.BookingDate
            };

            booking.IsDeleted = true;
            _unitOfWork.Bookings.UpdateBooking(booking);

            var unit = await _unitOfWork.Units.GetUnitByIdAsync(booking.UnitId);
            if (unit != null)
            {
                unit.Status = UnitStatus.Available;
                unit.BuyerId = null;
                unit.BookingId = null;

                var floor = await _unitOfWork.Floors.GetFloorByIdAsync(unit.FloorId);
                if (floor != null)
                {
                    var building = await _unitOfWork.Buildings.GetBuildingByIdAsync(floor.BuildingId);
                    if (building != null)
                    {
                        building.AvailableUnits++;
                        var project = await _unitOfWork.Projects.GetProjectByIdAsync(building.ProjectId);
                        if (project != null) project.AvailableUnits++;
                    }
                }
            }

            await _unitOfWork.CompleteAsync();

            await _activityLogService.LogActivityAsync(
                "حذف", "حجز", id,
                $"حذف الحجز رقم {id} — وحدة: {booking.UnitId} — عميل: {booking.BuyerId}",
                currentUserId,
                oldValues: oldSnapshot);

            return true;
        }
    }
}