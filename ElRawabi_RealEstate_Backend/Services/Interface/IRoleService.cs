using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.DTOs.Responses;

namespace ElRawabi_RealEstate_Backend.Services.Interface
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync();
        Task<RoleResponseDto?> GetRoleByIdAsync(int id);
        Task<RoleResponseDto> CreateRoleAsync(RoleRequestDto roleDto, int? currentUserId);
        Task<bool> UpdateRoleAsync(int id, RoleRequestDto roleDto, int? currentUserId);
        Task<bool> DeleteRoleAsync(int id, int? currentUserId);
    }
}
