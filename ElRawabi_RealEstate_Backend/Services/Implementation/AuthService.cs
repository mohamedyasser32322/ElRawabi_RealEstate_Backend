using ElRawabi_RealEstate_Backend.Dtos.Auth;
using ElRawabi_RealEstate_Backend.Helpers;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using ElRawabi_RealEstate_Backend.Services.Interface;

namespace ElRawabi_RealEstate_Backend.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto?> LoginUserAsync(LoginRequestDto loginDto)
        {
            var user = await _unitOfWork.Users.GetUserByEmailAsync(loginDto.Email);
            if (user == null || user.IsDeleted || !user.IsActive) return null;

            if (!PasswordHelper.VerifyPassword(loginDto.Password, user.HashPassword)) return null;

            var role = await _unitOfWork.Roles.GetRoleByIdAsync(user.RoleId);
            var roleName = role?.RoleName ?? "User";

            var token = JwtHelper.GenerateToken(user.Id, user.Email, roleName, _configuration);

            return new AuthResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = roleName,
                Token = token
            };
        }

        public async Task<AuthResponseDto?> LoginBuyerAsync(LoginRequestDto loginDto)
        {
            var buyer = await _unitOfWork.Buyers.GetBuyerByEmailAsync(loginDto.Email);
            if (buyer == null || buyer.IsDeleted) return null;

   
            if (!PasswordHelper.VerifyPassword(loginDto.Password, buyer.HashPassword)) return null;

            var token = JwtHelper.GenerateToken(buyer.Id, buyer.Email, "Buyer", _configuration);

            return new AuthResponseDto
            {
                Id = buyer.Id,
                Email = buyer.Email,
                Role = "Buyer",
                Token = token
            };
        }
    }
}
