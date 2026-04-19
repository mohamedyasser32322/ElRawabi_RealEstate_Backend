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

        public async Task<IEnumerable<BuildingResponseDto>> GetAllBuildingsAsync() => _mapper.Map<IEnumerable<BuildingResponseDto>>(await _unitOfWork.Buildings.GetAllBuildingsAsync());
        public async Task<IEnumerable<BuildingResponseDto>> GetBuildingsByProjectIdAsync(int projectId) => _mapper.Map<IEnumerable<BuildingResponseDto>>(await _unitOfWork.Buildings.GetBuildingsByProjectIdAsync(projectId));
        public async Task<BuildingResponseDto?> GetBuildingByIdAsync(int id) { var b = await _unitOfWork.Buildings.GetBuildingByIdAsync(id); return b == null ? null : _mapper.Map<BuildingResponseDto>(b); }

        public async Task<BuildingResponseDto> CreateBuildingAsync(BuildingRequestDto buildingDto, int? currentUserId)
        {
            var building = _mapper.Map<Building>(buildingDto);
            await _unitOfWork.Buildings.AddBuildingAsync(building);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("إضافة", "مبنى", building.Id, $"تم إنشاء مبنى جديد: {building.Name}", currentUserId);
            return _mapper.Map<BuildingResponseDto>(building);
        }

        public async Task<bool> UpdateBuildingAsync(int id, BuildingRequestDto buildingDto, int? currentUserId)
        {
            var building = await _unitOfWork.Buildings.GetBuildingByIdAsync(id);
            if (building == null) return false;
            _mapper.Map(buildingDto, building);
            _unitOfWork.Buildings.UpdateBuilding(building);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("تعديل", "مبنى", id, $"تم تعديل بيانات مبنى {building.Name}", currentUserId);
            return true;
        }

        public async Task<bool> DeleteBuildingAsync(int id, int? currentUserId)
        {
            var building = await _unitOfWork.Buildings.GetBuildingByIdAsync(id);
            if (building == null) return false;
            building.IsDeleted = true;
            _unitOfWork.Buildings.UpdateBuilding(building);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("حذف", "مبنى", id, $"تم حذف مبنى {building.Name}", currentUserId);
            return true;
        }
    }
}