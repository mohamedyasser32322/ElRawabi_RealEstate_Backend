using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.DTOs.Responses;

namespace ElRawabi_RealEstate_Backend.Services.Interface
{
    public interface IBuildingImageService
    {
        Task<IEnumerable<BuildingImageResponseDto>> GetAllBuildingImagesAsync();
        Task<BuildingImageResponseDto?> GetBuildingImageByIdAsync(int id);
        Task<BuildingImageResponseDto> CreateBuildingImageAsync(BuildingImageRequestDto imageDto);
        Task<bool> UpdateBuildingImageAsync(int id, BuildingImageRequestDto imageDto);
        Task<bool> DeleteBuildingImageAsync(int id);
    }
}
