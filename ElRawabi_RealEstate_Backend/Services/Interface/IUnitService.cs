using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.DTOs.Responses;

namespace ElRawabi_RealEstate_Backend.Services.Interface
{
    public interface IUnitService
    {
        Task<IEnumerable<UnitResponseDto>> GetAllUnitsAsync();
        Task<UnitResponseDto?> GetUnitByIdAsync(int id);
        Task<UnitResponseDto> CreateUnitAsync(UnitRequestDto unitDto, int? currentUserId);
        Task<bool> UpdateUnitAsync(int id, UnitRequestDto unitDto, int? currentUserId);
        Task<bool> DeleteUnitAsync(int id, int? currentUserId);
    }
}
