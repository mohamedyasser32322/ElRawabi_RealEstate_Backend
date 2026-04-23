using AutoMapper;
using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.DTOs.Responses;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using ElRawabi_RealEstate_Backend.Services.Interface;

namespace ElRawabi_RealEstate_Backend.Services.Implementation
{
    public class BuildingService : IBuildingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IActivityLogService _activityLogService;

        public BuildingService(IUnitOfWork unitOfWork, IMapper mapper, IActivityLogService activityLogService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _activityLogService = activityLogService;
        }

        public async Task<IEnumerable<BuildingResponseDto>> GetAllBuildingsAsync() =>
            _mapper.Map<IEnumerable<BuildingResponseDto>>(await _unitOfWork.Buildings.GetAllBuildingsAsync());

        public async Task<IEnumerable<BuildingResponseDto>> GetBuildingsByProjectIdAsync(int projectId) =>
            _mapper.Map<IEnumerable<BuildingResponseDto>>(await _unitOfWork.Buildings.GetBuildingsByProjectIdAsync(projectId));

        public async Task<BuildingResponseDto?> GetBuildingByIdAsync(int id)
        {
            var b = await _unitOfWork.Buildings.GetBuildingByIdAsync(id);
            return b == null ? null : _mapper.Map<BuildingResponseDto>(b);
        }

        public async Task<BuildingResponseDto> CreateBuildingAsync(BuildingRequestDto buildingDto, int? currentUserId)
        {
            var building = _mapper.Map<Building>(buildingDto);
            await _unitOfWork.Buildings.AddBuildingAsync(building);
            await _unitOfWork.CompleteAsync();

            var newSnapshot = new { building.Name, building.ProjectId, building.TotalUnits };

            await _activityLogService.LogActivityAsync(
                "إضافة", "مبنى", building.Id,
                $"إنشاء مبنى جديد: {building.Name}",
                currentUserId,
                newValues: newSnapshot);

            return _mapper.Map<BuildingResponseDto>(building);
        }

        public async Task<bool> UpdateBuildingAsync(int id, BuildingRequestDto buildingDto, int? currentUserId)
        {
            var building = await _unitOfWork.Buildings.GetBuildingByIdAsync(id);
            if (building == null) return false;

            var oldSnapshot = new { building.Name, building.ProjectId, building.TotalUnits };

            _mapper.Map(buildingDto, building);
            _unitOfWork.Buildings.UpdateBuilding(building);
            await _unitOfWork.CompleteAsync();

            var newSnapshot = new { building.Name, building.ProjectId, building.TotalUnits };

            await _activityLogService.LogActivityAsync(
                "تعديل", "مبنى", id,
                $"تعديل بيانات مبنى {building.Name}",
                currentUserId,
                oldValues: oldSnapshot,
                newValues: newSnapshot);

            return true;
        }

        public async Task<bool> DeleteBuildingAsync(int id, int? currentUserId)
        {
            var building = await _unitOfWork.Buildings.GetBuildingByIdAsync(id);
            if (building == null) return false;

            var oldSnapshot = new
            {
                building.Name,
                building.ProjectId,
                building.TotalUnits,
                FloorCount = building.Floors?.Count ?? 0
            };

            foreach (var floor in building.Floors)
            {
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
            }

            building.IsDeleted = true;
            _unitOfWork.Buildings.UpdateBuilding(building);
            await _unitOfWork.CompleteAsync();

            await _activityLogService.LogActivityAsync(
                "حذف", "مبنى", id,
                $"حذف مبنى {building.Name} مع جميع محتوياته",
                currentUserId,
                oldValues: oldSnapshot);

            return true;
        }
    }
}