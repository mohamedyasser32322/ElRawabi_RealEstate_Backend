using AutoMapper;
using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.DTOs.Responses;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using ElRawabi_RealEstate_Backend.Services.Interface;

namespace ElRawabi_RealEstate_Backend.Services.Implementation
{
    public class ConstructionStageService : IConstructionStageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IActivityLogService _activityLogService;

        public ConstructionStageService(IUnitOfWork unitOfWork, IMapper mapper, IActivityLogService activityLogService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _activityLogService = activityLogService;
        }

        public async Task<IEnumerable<ConstructionStageResponseDto>> GetAllConstructionStagesAsync() => _mapper.Map<IEnumerable<ConstructionStageResponseDto>>(await _unitOfWork.ConstructionStages.GetAllConstructionStagesAsync());
        public async Task<ConstructionStageResponseDto?> GetConstructionStageByIdAsync(int id) => _mapper.Map<ConstructionStageResponseDto>(await _unitOfWork.ConstructionStages.GetConstructionStageByIdAsync(id));

        public async Task<ConstructionStageResponseDto> CreateConstructionStageAsync(ConstructionStageRequestDto stageDto, int? currentUserId)
        {
            var stage = _mapper.Map<ConstructionStage>(stageDto);
            await _unitOfWork.ConstructionStages.AddConstructionStageAsync(stage);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("إضافة", "مرحلة بناء", stage.Id, $"تم إضافة مرحلة: {stage.StageName}", currentUserId);
            return _mapper.Map<ConstructionStageResponseDto>(stage);
        }

        public async Task<bool> UpdateConstructionStageAsync(int id, ConstructionStageRequestDto stageDto, int? currentUserId)
        {
            var stage = await _unitOfWork.ConstructionStages.GetConstructionStageByIdAsync(id);
            if (stage == null) return false;
            _mapper.Map(stageDto, stage);
            _unitOfWork.ConstructionStages.UpdateConstructionStage(stage);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("تعديل", "مرحلة بناء", id, $"تم تعديل مرحلة: {stage.StageName}", currentUserId);
            return true;
        }

        public async Task<bool> DeleteConstructionStageAsync(int id, int? currentUserId)
        {
            var stage = await _unitOfWork.ConstructionStages.GetConstructionStageByIdAsync(id);
            if (stage == null) return false;
            stage.IsDeleted = true;
            _unitOfWork.ConstructionStages.UpdateConstructionStage(stage);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("حذف", "مرحلة بناء", id, $"تم حذف مرحلة: {stage.StageName}", currentUserId);
            return true;
        }
    }
}