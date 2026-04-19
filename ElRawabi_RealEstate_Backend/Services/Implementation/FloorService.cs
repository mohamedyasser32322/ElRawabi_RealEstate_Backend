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

        public async Task<IEnumerable<FloorResponseDto>> GetAllFloorsAsync() => _mapper.Map<IEnumerable<FloorResponseDto>>(await _unitOfWork.Floors.GetAllFloorsAsync());
        public async Task<FloorResponseDto?> GetFloorByIdAsync(int id) => _mapper.Map<FloorResponseDto>(await _unitOfWork.Floors.GetFloorByIdAsync(id));

        public async Task<FloorResponseDto> CreateFloorAsync(FloorRequestDto floorDto, int? currentUserId)
        {
            var floor = _mapper.Map<Floor>(floorDto);
            await _unitOfWork.Floors.AddFloorAsync(floor);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("إضافة", "دور", floor.Id, $"تم إضافة دور رقم {floor.FloorNumber}", currentUserId);
            return _mapper.Map<FloorResponseDto>(floor);
        }

        public async Task<bool> UpdateFloorAsync(int id, FloorRequestDto floorDto, int? currentUserId)
        {
            var floor = await _unitOfWork.Floors.GetFloorByIdAsync(id);
            if (floor == null) return false;
            _mapper.Map(floorDto, floor);
            _unitOfWork.Floors.UpdateFloor(floor);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("تعديل", "دور", id, $"تم تعديل بيانات الدور رقم {floor.FloorNumber}", currentUserId);
            return true;
        }

        public async Task<bool> DeleteFloorAsync(int id, int? currentUserId)
        {
            var floor = await _unitOfWork.Floors.GetFloorByIdAsync(id);
            if (floor == null) return false;
            floor.IsDeleted = true;
            _unitOfWork.Floors.UpdateFloor(floor);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("حذف", "دور", id, $"تم حذف الدور رقم {floor.FloorNumber}", currentUserId);
            return true;
        }
    }
}