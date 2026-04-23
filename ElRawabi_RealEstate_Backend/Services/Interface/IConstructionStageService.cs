using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.DTOs.Responses;

namespace ElRawabi_RealEstate_Backend.Services.Interface
{
    public interface IConstructionStageService
    {
        Task<IEnumerable<ConstructionStageResponseDto>> GetAllConstructionStagesAsync();

        Task<IEnumerable<ConstructionStageResponseDto>> GetByBuildingIdAsync(int buildingId);

        Task<ConstructionStageResponseDto?> GetConstructionStageByIdAsync(int id);

        Task<ConstructionStageResponseDto> CreateConstructionStageAsync(
            ConstructionStageRequestDto dto, int? currentUserId);

        Task<bool> UpdateConstructionStageAsync(
            int id, ConstructionStageRequestDto dto, int? currentUserId);

        Task<bool> DeleteConstructionStageAsync(int id, int? currentUserId);
    }
}