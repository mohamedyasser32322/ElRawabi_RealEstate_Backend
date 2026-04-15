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
        public async Task<BuildingResponseDto?> GetBuildingByIdAsync(int id) => _mapper.Map<BuildingResponseDto>(await _unitOfWork.Buildings.GetBuildingByIdAsync(id));

        public async Task<BuildingResponseDto> CreateBuildingAsync(BuildingRequestDto buildingDto)
        {
            var building = _mapper.Map<Building>(buildingDto);
            await _unitOfWork.Buildings.AddBuildingAsync(building);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("إضافة", "عمارة", building.Id, $"تم إضافة عمارة جديدة: {building.Name}", null);
            return _mapper.Map<BuildingResponseDto>(building);
        }

        public async Task<bool> UpdateBuildingAsync(int id, BuildingRequestDto buildingDto)
        {
            var building = await _unitOfWork.Buildings.GetBuildingByIdAsync(id);
            if (building == null) return false;
            _mapper.Map(buildingDto, building);
            _unitOfWork.Buildings.UpdateBuilding(building);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("تعديل", "عمارة", id, $"تم تعديل بيانات العمارة {building.Name}", null);
            return true;
        }

        public async Task<bool> DeleteBuildingAsync(int id)
        {
            var building = await _unitOfWork.Buildings.GetBuildingByIdAsync(id);
            if (building == null) return false;
            building.IsDeleted = true;
            _unitOfWork.Buildings.UpdateBuilding(building);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("حذف", "عمارة", id, $"تم حذف العمارة {building.Name}", null);
            return true;
        }
    }
}
