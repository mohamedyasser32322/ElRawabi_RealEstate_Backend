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

        public ConstructionStageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ConstructionStageResponseDto>> GetAllConstructionStagesAsync()
        {
            var stages = await _unitOfWork.ConstructionStages.GetAllConstructionStagesAsync();
            return _mapper.Map<IEnumerable<ConstructionStageResponseDto>>(stages);
        }

        public async Task<ConstructionStageResponseDto?> GetConstructionStageByIdAsync(int id)
        {
            var stage = await _unitOfWork.ConstructionStages.GetConstructionStageByIdAsync(id);
            if (stage == null) return null;
            return _mapper.Map<ConstructionStageResponseDto>(stage);
        }

        public async Task<ConstructionStageResponseDto> CreateConstructionStageAsync(ConstructionStageRequestDto stageDto)
        {
            var stage = _mapper.Map<ConstructionStage>(stageDto);
            await _unitOfWork.ConstructionStages.AddConstructionStageAsync(stage);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<ConstructionStageResponseDto>(stage);
        }

        public async Task<bool> UpdateConstructionStageAsync(int id, ConstructionStageRequestDto stageDto)
        {
            var stage = await _unitOfWork.ConstructionStages.GetConstructionStageByIdAsync(id);
            if (stage == null) return false;

            _mapper.Map(stageDto, stage);
            _unitOfWork.ConstructionStages.UpdateConstructionStage(stage);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteConstructionStageAsync(int id)
        {
            var stage = await _unitOfWork.ConstructionStages.GetConstructionStageByIdAsync(id);
            if (stage == null) return false;

            stage.IsDeleted = true;
            _unitOfWork.ConstructionStages.UpdateConstructionStage(stage);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
