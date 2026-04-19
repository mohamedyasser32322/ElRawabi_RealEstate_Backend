using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.DTOs.Responses;

namespace ElRawabi_RealEstate_Backend.Services.Interface
{
    public interface IFloorService
    {
        Task<IEnumerable<FloorResponseDto>> GetAllFloorsAsync();
        Task<FloorResponseDto?> GetFloorByIdAsync(int id);
        Task<FloorResponseDto> CreateFloorAsync(FloorRequestDto floorDto, int? currentUserId);
        Task<bool> UpdateFloorAsync(int id, FloorRequestDto floorDto, int? currentUserId);
        Task<bool> DeleteFloorAsync(int id, int? currentUserId);
    }
}
