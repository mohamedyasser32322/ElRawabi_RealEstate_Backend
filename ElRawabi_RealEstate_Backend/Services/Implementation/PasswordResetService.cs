using ElRawabi_RealEstate_Backend.Repositories.Interface;
using ElRawabi_RealEstate_Backend.Services.Interface;
using Microsoft.Extensions.Caching.Memory;

namespace ElRawabi_RealEstate_Backend.Services.Implementation
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly IMemoryCache _cache;

        public PasswordResetService(IUnitOfWork unitOfWork, IEmailService emailService, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _cache = cache;
        }

        public async Task<bool> SendResetTokenAsync(string email)
        {
            var user = await _unitOfWork.Users.GetUserByEmailAsync(email);
            var buyer = user == null
                ? await _unitOfWork.Buyers.GetBuyerByEmailAsync(email)
                : null;

            if (user == null && buyer == null) return false;

            var token = Guid.NewGuid().ToString("N")[..8].ToUpper();
            _cache.Set($"reset_{email}", token, TimeSpan.FromMinutes(15));
            await _emailService.SendPasswordResetEmailAsync(email, token);
            return true;
        }


        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {
            if (!_cache.TryGetValue($"reset_{email}", out string? savedToken))
                return false;

            if (savedToken != token.ToUpper())
                return false;

            var hashed = BCrypt.Net.BCrypt.HashPassword(newPassword);

            var user = await _unitOfWork.Users.GetUserByEmailAsync(email);
            if (user != null)
            {
                user.HashPassword = hashed;
                _unitOfWork.Users.UpdateUser(user);
            }
            else
            {
                var buyer = await _unitOfWork.Buyers.GetBuyerByEmailAsync(email);
                if (buyer == null) return false;
                buyer.HashPassword = hashed;
                _unitOfWork.Buyers.UpdateBuyer(buyer);
            }

            await _unitOfWork.CompleteAsync();
            _cache.Remove($"reset_{email}");
            return true;
        }
    }
}