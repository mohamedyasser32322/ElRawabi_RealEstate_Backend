using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.DTOs.Responses;

namespace ElRawabi_RealEstate_Backend.Services.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        Task<UserResponseDto?> GetUserByIdAsync(int id);
        Task<UserResponseDto> CreateUserAsync(UserCreateRequestDto dto, int? currentUserId);
        Task<bool> UpdateUserAsync(int id, UserUpdateRequestDto dto, int? currentUserId);
        Task<bool> DeleteUserAsync(int id, int? currentUserId);
    }
}
