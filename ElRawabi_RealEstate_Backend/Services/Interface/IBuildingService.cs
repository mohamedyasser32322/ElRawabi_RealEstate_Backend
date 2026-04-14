using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.DTOs.Responses;

namespace ElRawabi_RealEstate_Backend.Services.Interface
{
    public interface IBuildingService
    {
        Task<IEnumerable<BuildingResponseDto>> GetAllBuildingsAsync();
        Task<BuildingResponseDto?> GetBuildingByIdAsync(int id);
        Task<BuildingResponseDto> CreateBuildingAsync(BuildingRequestDto buildingDto);
        Task<bool> UpdateBuildingAsync(int id, BuildingRequestDto buildingDto);
        Task<bool> DeleteBuildingAsync(int id);
    }
}
