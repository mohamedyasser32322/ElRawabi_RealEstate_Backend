using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.DTOs.Responses;

namespace ElRawabi_RealEstate_Backend.Services.Interface
{
    public interface IConstructionStageService
    {
        Task<IEnumerable<ConstructionStageResponseDto>> GetAllConstructionStagesAsync();
        Task<ConstructionStageResponseDto?> GetConstructionStageByIdAsync(int id);
        Task<ConstructionStageResponseDto> CreateConstructionStageAsync(ConstructionStageRequestDto stageDto);
        Task<bool> UpdateConstructionStageAsync(int id, ConstructionStageRequestDto stageDto);
        Task<bool> DeleteConstructionStageAsync(int id);
    }
}
