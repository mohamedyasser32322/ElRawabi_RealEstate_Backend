using AutoMapper;
using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.DTOs.Responses;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using ElRawabi_RealEstate_Backend.Services.Interface;

namespace ElRawabi_RealEstate_Backend.Services.Implementation
{
    public class FloorService : IFloorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IActivityLogService _activityLogService;

        public FloorService(IUnitOfWork unitOfWork, IMapper mapper, IActivityLogService activityLogService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _activityLogService = activityLogService;
        }

        public async Task<IEnumerable<FloorResponseDto>> GetAllFloorsAsync() =>
            _mapper.Map<IEnumerable<FloorResponseDto>>(await _unitOfWork.Floors.GetAllFloorsAsync());

        public async Task<FloorResponseDto?> GetFloorByIdAsync(int id) =>
            _mapper.Map<FloorResponseDto>(await _unitOfWork.Floors.GetFloorByIdAsync(id));

        public async Task<FloorResponseDto> CreateFloorAsync(FloorRequestDto floorDto, int? currentUserId)
        {
            var floor = _mapper.Map<Floor>(floorDto);
            await _unitOfWork.Floors.AddFloorAsync(floor);
            await _unitOfWork.CompleteAsync();

            var newSnapshot = new { floor.FloorNumber, floor.BuildingId };

            await _activityLogService.LogActivityAsync(
                "إضافة", "دور", floor.Id,
                $"إضافة دور رقم {floor.FloorNumber}",
                currentUserId,
                newValues: newSnapshot);

            return _mapper.Map<FloorResponseDto>(floor);
        }

        public async Task<bool> UpdateFloorAsync(int id, FloorRequestDto floorDto, int? currentUserId)
        {
            var floor = await _unitOfWork.Floors.GetFloorByIdAsync(id);
            if (floor == null) return false;

            var oldSnapshot = new { floor.FloorNumber, floor.BuildingId };

            _mapper.Map(floorDto, floor);
            _unitOfWork.Floors.UpdateFloor(floor);
            await _unitOfWork.CompleteAsync();

            var newSnapshot = new { floor.FloorNumber, floor.BuildingId };

            await _activityLogService.LogActivityAsync(
                "تعديل", "دور", id,
                $"تعديل بيانات الدور رقم {floor.FloorNumber}",
                currentUserId,
                oldValues: oldSnapshot,
                newValues: newSnapshot);

            return true;
        }

        public async Task<bool> DeleteFloorAsync(int id, int? currentUserId)
        {
            var floor = await _unitOfWork.Floors.GetFloorByIdAsync(id);
            if (floor == null) return false;

            var oldSnapshot = new { floor.FloorNumber, floor.BuildingId, UnitCount = floor.Units?.Count ?? 0 };

            foreach (var unit in floor.Units)
            {
                if (unit.Booking != null)
                {
                    unit.Booking.IsDeleted = true;
                    _unitOfWork.Bookings.UpdateBooking(unit.Booking);
                }
                unit.IsDeleted = true;
                _unitOfWork.Units.UpdateUnit(unit);
            }

            floor.IsDeleted = true;
            _unitOfWork.Floors.UpdateFloor(floor);
            await _unitOfWork.CompleteAsync();

            await _activityLogService.LogActivityAsync(
                "حذف", "دور", id,
                $"حذف الدور رقم {floor.FloorNumber} مع جميع وحداته",
                currentUserId,
                oldValues: oldSnapshot);

            return true;
        }
    }
}