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
            // ابحث في Users أو Buyers
            var user = (await _unitOfWork.Users.GetAllUsersAsync()).FirstOrDefault(u => u.Email == email);
            var buyer = user == null
                ? (await _unitOfWork.Buyers.GetAllBuyersAsync()).FirstOrDefault(b => b.Email == email)
                : null;

            if (user == null && buyer == null) return false;

            // توكن عشوائي
            var token = Guid.NewGuid().ToString("N")[..8].ToUpper();

            // احفظه في الـ cache لمدة 15 دقيقة
            _cache.Set($"reset_{email}", token, TimeSpan.FromMinutes(15));

            await _emailService.SendPasswordResetEmailAsync(email, token);
            return true;
        }

        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {
            // تحقق من التوكن
            if (!_cache.TryGetValue($"reset_{email}", out string? savedToken))
                return false;

            if (savedToken != token.ToUpper())
                return false;

            var hashed = BCrypt.Net.BCrypt.HashPassword(newPassword);

            // حدّث كلمة السر في Users أو Buyers
            var users = await _unitOfWork.Users.GetAllUsersAsync();
            var user = users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                user.HashPassword = hashed;
                _unitOfWork.Users.UpdateUser(user);
            }
            else
            {
                var buyers = await _unitOfWork.Buyers.GetAllBuyersAsync();
                var buyer = buyers.FirstOrDefault(b => b.Email == email);
                if (buyer == null) return false;
                buyer.HashPassword = hashed;
                _unitOfWork.Buyers.UpdateBuyer(buyer);
            }

            await _unitOfWork.CompleteAsync();

            // امسح التوكن
            _cache.Remove($"reset_{email}");
            return true;
        }
    }
}