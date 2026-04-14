using ElRawabi_RealEstate_Backend.Dtos.Auth;

namespace ElRawabi_RealEstate_Backend.Services.Interface
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> LoginUserAsync(LoginRequestDto loginDto);
        Task<AuthResponseDto?> LoginBuyerAsync(LoginRequestDto loginDto);
    }
}
